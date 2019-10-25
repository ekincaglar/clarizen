using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Ekin.Log;
using Newtonsoft.Json.Linq;

namespace Ekin.Clarizen
{
    /// <summary>
    /// .Net wrapper for the Clarizen API v2.0 located at https://api.clarizen.com/V2.0/services
    /// Developed by Ekin Caglar - ekin@caglar.com
    /// October 2016 - August 2019
    /// </summary>
    public class API
    {
        #region Public properties

        public bool removeInvalidFieldsFromJsonResult { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public string serverLocation { get; set; }
        public string sessionId { get; set; }

        public bool isSandbox { get; set; } = false;
        public int TotalAPICallsMadeInCurrentSession { get; set; }
        public bool serializeNullValues { get; set; } = false;
        public int retry { get; set; } = 1;
        public int sleepBetweenRetries { get; set; } = 0;

        public LogFactory Logs { get; set; }

        public BulkOperations Bulk { get; private set; }
        public FileUploadHelper FileUpload { get; private set; }

        public bool isBulk { get; private set; }
        private List<request> bulkRequests { get; set; }

        public int? timeout { get; set; } = 120000;

        #endregion

        /// <summary>
        /// To use Clarizen API, first call API.Login("yourUserName", "yourPassword") 
        /// If this method returns TRUE you can start using the helper functions provided in this class, such as DescribeMetadata, CreateObject, UpdateObject, DeleteObject and ExecuteQuery.
        /// When finished, don't forget to call API.Logout()
        /// </summary>
        public API()
        {
            TotalAPICallsMadeInCurrentSession = 0;
            Logs = new LogFactory();
            Bulk = new BulkOperations(this);
            FileUpload = new FileUploadHelper(this);
        }

        /// <summary>
        /// When the API class is initiated as static, to use Bulk queries you need to clone it after calling Login().
        /// </summary>
        /// <returns></returns>
        public API Clone()
        {
            return new API()
            {
                sessionId = this.sessionId,
                isSandbox = this.isSandbox,
                serverLocation = this.serverLocation,
                username = this.username,
                password = this.password,
                retry = this.retry,
                sleepBetweenRetries = this.sleepBetweenRetries,
                serializeNullValues = this.serializeNullValues,
                timeout = this.timeout,
                removeInvalidFieldsFromJsonResult = this.removeInvalidFieldsFromJsonResult
                // We don't copy Logs or Bulk in the clone
            };
        }

        #region Authentication methods

        /// <summary>
        /// Opens a new session using the credentials given for accessing Clarizen API
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string username, string password)
        {
            // First we get the Url where your organization API is located
            Authentication.getServerDefinition serverDefinition = new Authentication.getServerDefinition(new Ekin.Clarizen.Authentication.Request.getServerDefinition(username, password, new Ekin.Clarizen.Authentication.Request.loginOptions()), isSandbox);
            //TotalAPICallsMadeInCurrentSession++; // Login call doesn't count towards quota (Confirmed by Yaron Perlman on 9 Nov 2016)
            Logs.Assert(serverDefinition.IsCalledSuccessfully, "Ekin.Clarizen.API", "Login", "Server definition could not be retrieved", serverDefinition.Error);
            if (serverDefinition.IsCalledSuccessfully)
            {
                serverLocation = serverDefinition.Data.serverLocation;

                // Then we login to the API at the above location
                Authentication.login CZlogin = new Authentication.login(serverLocation, new Ekin.Clarizen.Authentication.Request.login(username, password, new Ekin.Clarizen.Authentication.Request.loginOptions()));
                Logs.Assert(CZlogin.IsCalledSuccessfully, "Ekin.Clarizen.API", "Login", "Login failed", CZlogin.Error);
                if (CZlogin.IsCalledSuccessfully)
                {
                    // Upon successful login a unique ID representing the current session is returned.
                    // This ID is then passed on to all the API calls in the Http Header
                    sessionId = CZlogin.Data.sessionId;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Loges the user out and closes the current session
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            Authentication.logout CZlogout = new Authentication.logout(serverLocation, sessionId);
            //TotalAPICallsMadeInCurrentSession++; // Logout call doesn't count towards quota (Confirmed by Yaron Perlman on 9 Nov 2016)
            Logs.Assert(CZlogout.IsCalledSuccessfully, "Ekin.Clarizen.API", "Logout", "Logout failed");
            return (CZlogout.IsCalledSuccessfully);
        }

        /// <summary>
        /// In a multi-org environment this endpoint allows administrators at the master org to set a password for any user in the system. It changes passwords without sending any email.
        /// </summary>
        /// <param name="userId">Fully qualified Id of the user. Example: /Organization/SomeOrgId/User/SomeUserId</param>
        /// <param name="newPassword">New password for the user. The Password will be checked for complexity and for history. The user will not be required to change the password after login.</param>
        /// <returns></returns>
        public bool SetPassword(string userId, string newPassword)
        {
            Authentication.setPassword op = new Authentication.setPassword(new Authentication.Request.setPassword(userId, newPassword), CallSettings.GetFromAPI(this));
            Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "setPassword", op.Error);
            return (op.IsCalledSuccessfully);
        }

        /// <summary>
        /// Returns information about the current session
        /// </summary>
        /// <returns></returns>
        public Authentication.getSessionInfo GetSessionInfo()
        {
            Authentication.getSessionInfo sessionInfo = new Authentication.getSessionInfo(serverLocation, sessionId);
            TotalAPICallsMadeInCurrentSession++;
            Logs.Assert(sessionInfo.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetSessionInfo", "Session info could be retrieved", sessionInfo.Error);
            return sessionInfo;
        }

        #endregion

        #region Metadata methods

        /// <summary>
        /// Returns information about the entity types in your organization
        /// </summary>
        /// <param name="typeNames"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Metadata.describeMetadata DescribeMetadata(string[] typeNames, string[] flags)
        {
            Metadata.describeMetadata metadata = new Metadata.describeMetadata(
                ((typeNames == null && flags == null) ? new Ekin.Clarizen.Metadata.Request.describeMetadata() :
                                                        new Ekin.Clarizen.Metadata.Request.describeMetadata(typeNames, flags)), 
                CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(metadata.BulkRequest);
            else { Logs.Assert(metadata.IsCalledSuccessfully, "Ekin.Clarizen.API", "DescribeMetadata", "describeMetadata failed", metadata.Error); TotalAPICallsMadeInCurrentSession++; }
            return metadata;
        }

        /// <summary>
        /// Returns information about the entity types in your organization
        /// </summary>
        /// <param name="typeNames"></param>
        /// <returns></returns>
        public Metadata.describeMetadata DescribeMetadata(string[] typeNames)
        {
            return DescribeMetadata(typeNames, null);
        }

        /// <summary>
        /// Returns information about the entity types in your organization
        /// </summary>
        /// <returns></returns>
        public Metadata.describeMetadata DescribeMetadata()
        {
            return DescribeMetadata(null, null);
        }

        /// <summary>
        /// Returns the list of entity types available for your organization
        /// </summary>
        /// <returns></returns>
        public Metadata.listEntities ListEntities()
        {
            Metadata.listEntities metadata = new Metadata.listEntities(CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(metadata.BulkRequest);
            else { Logs.Assert(metadata.IsCalledSuccessfully, "Ekin.Clarizen.API", "ListEntities", "listEntities failed", metadata.Error); TotalAPICallsMadeInCurrentSession++; }
            return metadata;
        }

        /// <summary>
        /// Returns information about an Entity Type in Clarizen, including its fields, relations and states
        /// </summary>
        /// <param name="typeNames"></param>
        /// <returns></returns>
        public Metadata.describeEntities DescribeEntities(string[] typeNames)
        {
            Metadata.describeEntities metadata = new Metadata.describeEntities(new Metadata.Request.describeEntities(typeNames), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(metadata.BulkRequest);
            else { Logs.Assert(metadata.IsCalledSuccessfully, "Ekin.Clarizen.API", "DescribeEntities", "describeEntities failed", metadata.Error); TotalAPICallsMadeInCurrentSession++; }
            return metadata;
        }

        /// <summary>
        /// Describes the relation between entities
        /// </summary>
        /// <param name="typeNames"></param>
        /// <returns></returns>
        public Metadata.describeEntityRelations DescribeEntityRelations(string[] typeNames)
        {
            Metadata.describeEntityRelations metadata = new Metadata.describeEntityRelations(new Metadata.Request.describeEntityRelations(typeNames), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(metadata.BulkRequest);
            else { Logs.Assert(metadata.IsCalledSuccessfully, "Ekin.Clarizen.API", "DescribeEntityRelations", "describeEntityRelations failed", metadata.Error); TotalAPICallsMadeInCurrentSession++; TotalAPICallsMadeInCurrentSession++; }
            return metadata;
        }

        /// <summary>
        /// Create a workflow rule in Clarizen.
        /// </summary>
        public Metadata.objects_put CreateWorkflowRule(string forType, string name, string description, string triggerType, string criteria, string action_url, string action_method, string action_headers, string action_body)
        {
            return CreateWorkflowRule(new Metadata.Request.objects_put(forType, name, description, triggerType, criteria, new action(action_url, action_method, action_headers, action_body)));
        }

        /// <summary>
        /// Create a workflow rule in Clarizen.
        /// </summary>
        public Metadata.objects_put CreateWorkflowRule(string forType, string name, string description, string triggerType, string criteria, action action)
        {
            return CreateWorkflowRule(new Metadata.Request.objects_put(forType, name, description, triggerType, criteria, action));
        }

        /// <summary>
        /// Create a workflow rule in Clarizen.
        /// </summary>
        public Metadata.objects_put CreateWorkflowRule(Metadata.Request.objects_put request)
        {
            Metadata.objects_put objects = new Metadata.objects_put(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "CreateWorkflowRule", "CreateWorkflowRule call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Delete an workflow rule in Clarizen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Metadata.objects_delete DeleteWorkflowRule(string id)
        {
            Metadata.objects_delete objects = new Metadata.objects_delete(new Metadata.Request.objects_delete(id), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "DeleteWorkflowRule", "DeleteWorkflowRule call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Retrieves the values of a system settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Metadata.getSystemSettingsValues GetSystemSettingsValues(string[] settings)
        {
            Metadata.getSystemSettingsValues metadata = new Metadata.getSystemSettingsValues(new Metadata.Request.getSystemSettingsValues(settings), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(metadata.BulkRequest);
            else { Logs.Assert(metadata.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetSystemSettingsValues", "getSystemSettingsValues failed", metadata.Error); TotalAPICallsMadeInCurrentSession++; }
            return metadata;
        }

        /// <summary>
        /// Save the values of a system settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public Metadata.setSystemSettingsValues SetSystemSettingsValues(fieldValue[] settings)
        {
            Metadata.setSystemSettingsValues metadata = new Metadata.setSystemSettingsValues(new Metadata.Request.setSystemSettingsValues(settings), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(metadata.BulkRequest);
            else { Logs.Assert(metadata.IsCalledSuccessfully, "Ekin.Clarizen.API", "SetSystemSettingsValues", "setSystemSettingsValues call failed", metadata.Error); TotalAPICallsMadeInCurrentSession++; }
            return metadata;
        }

        #endregion

        #region Data - CRUD methods

        /// <summary>
        /// Read an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to get</param>
        /// <param name="fields">The list of fields to read</param>
        /// <returns></returns>
        public Data.objects_get GetObject(string id, string[] fields)
        {
            Data.objects_get objects = new Data.objects_get(new Data.Request.objects_get(id, fields), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetObject", "objects_get call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Read an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to get</param>
        /// <returns></returns>
        public Data.objects_get GetObject(string id)
        {
            return GetObject(id: id, fields: null);
        }

        /// <summary>
        /// Read an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to get</param>
        /// <param name="pocoObject">Return type of the operation</param>
        /// <returns></returns>
        public dynamic GetObject(string id, Type pocoObject)
        {
            string[] fields = pocoObject.GetPropertyList();
            Data.objects_get objects = new Data.objects_get(new Data.Request.objects_get(id, fields), CallSettings.GetFromAPI(this), true);
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetObject", "objects_get call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            if (objects.IsCalledSuccessfully)
            {
                try
                {
                    Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(objects.Data);
                    RemoveInvalidFields(obj);
                    return obj.ToObject(pocoObject);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create an entity in Clarizen.
        /// </summary>
        /// <param name="id">When creating an entity, a unique ID is generated and returned as part of the object creation process. e.g. pass /User as id. If needed, you can also set a specific ID to the entity being created as long as you can guarantee this ID is unique, e.g. /User/dc84ee38-12cc-492e-b70d-d7fd660f4ae7.</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Data.objects_put CreateObject(string id, object obj)
        {
            Data.objects_put objects = new Data.objects_put(id, obj, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "CreateObject", "objects_put call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Update an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to update</param>
        /// <param name="obj">Object to update</param>
        /// <returns></returns>
        public Data.objects_post UpdateObject(string id, object obj)
        {
            Data.objects_post objects = new Data.objects_post(id, obj, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "UpdateObject", "objects_post call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Delete an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to delete</param>
        /// <returns></returns>
        public Data.objects_delete DeleteObject(string id)
        {
            Data.objects_delete objects = new Data.objects_delete(new Ekin.Clarizen.Data.Request.objects_delete(id), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "DeleteObject", "objects_delete call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Creates and immediatly retrieves an entity 
        /// </summary>
        /// <param name="entity">Entity to be created. Should include Id field such as /Task</param>
        /// <param name="fields">List of fields to retrieve</param>
        /// <returns></returns>
        public Data.createAndRetrieve CreateAndRetrieve(object entity, string[] fields)
        {
            Data.createAndRetrieve op = new Data.createAndRetrieve(new Data.Request.createAndRetrieve(entity, fields), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "CreateAndRetrieve", "createAndRetrieve call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Retrieves multiple entities from the same entity type in a single request 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.retrieveMultiple RetrieveMultiple(Data.Request.retrieveMultiple request)
        {
            Data.retrieveMultiple op = new Data.retrieveMultiple(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "RetrieveMultiple", "retrieveMultiple call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Retrieves multiple entities from the same entity type in a single request
        /// </summary>
        /// <param name="fields">Fields to retrieve</param>
        /// <param name="ids">Entity Ids</param>
        /// <returns></returns>
        public Data.retrieveMultiple RetrieveMultiple(string[] fields, string[] ids)
        {
            return RetrieveMultiple(new Data.Request.retrieveMultiple(fields, ids));
        }

        /// <summary>
        /// Gets all instances of an entity from Clarizen recursively
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pocoObject"></param>
        /// <returns></returns>
        public GetAllResult GetAll(string entityName, Type pocoObject, ICondition condition = null, int? pageSize = null, int? sleepTime = null)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { pocoObject });
            System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(listType);
            GetAllResult result = new GetAllResult()
            {
                Errors = new List<error>() { }
            };
            paging paging = new paging();
            paging.limit = pageSize.GetValueOrDefault(0) > 0 ? pageSize.GetValueOrDefault(0) : 1000;
            bool hasMore = true;
            while (hasMore)
            {
                Data.Queries.entityQuery request =
                    new Data.Queries.entityQuery(entityName, pocoObject.GetPropertyList(), null, null, null, false, false, paging);
                if (condition != null)
                    request.where = condition;
                Data.entityQuery entityQuery = EntityQuery(request);
                if (entityQuery.IsCalledSuccessfully)
                {
                    foreach (Newtonsoft.Json.Linq.JObject obj in entityQuery.Data.entities)
                    {
                        RemoveInvalidFields(obj);
                        list.Add(obj.ToObject(pocoObject));
                    }
                    paging = entityQuery.Data.paging;
                    hasMore = entityQuery.Data.paging.hasMore;
                }
                else
                {
                    result.Errors.Add(new error("", "Entity query failed with error: " + entityQuery.Error));
                    hasMore = false;
                }
                if (sleepTime.GetValueOrDefault() > 0)
                {
                    System.Threading.Thread.Sleep(sleepTime.GetValueOrDefault());
                }
            }
            result.Data = list;
            return result;
        }

        /// <summary>
        /// Executes a query and returns all results recursively
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pocoObject"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public GetAllResult GetAll(Interfaces.IClarizenQuery query, Type pocoObject, int? pageSize = null, int? sleepTime = null)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { pocoObject });
            System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(listType);
            GetAllResult result = new GetAllResult()
            {
                Errors = new List<error>() { }
            };
            paging paging = new paging();
            paging.limit = pageSize.GetValueOrDefault(0) > 0 ? pageSize.GetValueOrDefault(0) : 1000;
            bool hasMore = true;
            while (hasMore)
            {
                CallSettings callSettings = CallSettings.GetFromAPI(this);
                callSettings.isBulk = false;
                Data.query Query = new Data.query(new Data.Request.query(query.ToCZQL(), paging), callSettings);
                if (Query.IsCalledSuccessfully)
                {
                    foreach (Newtonsoft.Json.Linq.JObject obj in Query.Data.entities)
                    {
                        RemoveInvalidFields(obj);
                        list.Add(obj.ToObject(pocoObject));
                    }
                    paging = Query.Data.paging;
                    hasMore = Query.Data.paging.hasMore;
                }
                else
                {
                    string message = "Query failed with error" + (callSettings.retry > 1 ? $" after {callSettings.retry} retries" : "") + ": " + Query.Error;
                    result.Errors.Add(new error("", message));
                    hasMore = false;
                }
                if (sleepTime.GetValueOrDefault() > 0)
                {
                    System.Threading.Thread.Sleep(sleepTime.GetValueOrDefault());
                }
            }
            result.Data = list;
            return result;
        }

        public GetAllResult GetAllByFields(string entityName, string[] fields, ICondition condition = null, int? pageSize = null, int? sleepTime = null)
        {
            List<dynamic> list = new List<dynamic>();
            GetAllResult result = new GetAllResult()
            {
                Errors = new List<error>() { }
            };
            paging paging = new paging();
            paging.limit = pageSize.GetValueOrDefault(0) > 0 ? pageSize.GetValueOrDefault(0) : 5000;
            bool hasMore = true;
            while (hasMore)
            {
                Ekin.Clarizen.Data.Queries.entityQuery request = new Ekin.Clarizen.Data.Queries.entityQuery(entityName, fields, null, null, null, false, false, paging);
                if (condition != null)
                    request.where = condition;
                Ekin.Clarizen.Data.entityQuery entityQuery = EntityQuery(request);
                if (entityQuery.IsCalledSuccessfully)
                {
                    foreach (Newtonsoft.Json.Linq.JObject obj in entityQuery.Data.entities)
                    {
                        list.Add(obj);
                    }
                    paging = entityQuery.Data.paging;
                    hasMore = entityQuery.Data.paging.hasMore;
                }
                else
                {
                    result.Errors.Add(new error("", "Entity query failed with error: " + entityQuery.Error));
                    hasMore = false;
                }
                if (sleepTime.GetValueOrDefault() > 0)
                {
                    System.Threading.Thread.Sleep(sleepTime.GetValueOrDefault());
                }
            }
            result.Data = list;
            return result;
        }

        private void RemoveInvalidFields(JObject obj)
        {
            if (removeInvalidFieldsFromJsonResult)
            {
                //ignore the fields which have isError text as a value
                if (obj.Properties().Any(i => i.Value != null && i.Value.ToString().Contains("isError")))
                {
                    var allProperties = obj.Properties().ToList();
                    var count = allProperties?.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var property = allProperties[i];
                        if (property.Value != null && property.Value.ToString().Contains("isError"))
                        {
                            obj.Property(property.Name).Remove();
                        }
                    }
                }
            }
        }

        #endregion

        #region Data - Query methods

        /// <summary>
        /// Performs a query and returns the result count 
        /// </summary>
        /// <param name="query">findUserQuery, entityQuery, expenseQuery, timesheetQuery, aggregateQuery, relationQuery, cZQLQuery, entityFeedQuery, groupsQuery, newsFeedQuery, repliesQuery</param>
        /// <returns></returns>
        public Data.countQuery Count(IQuery query)
        {
            Data.countQuery entities = new Data.countQuery(new Data.Request.countQuery(query), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(entities.BulkRequest);
            else { Logs.Assert(entities.IsCalledSuccessfully, "Ekin.Clarizen.API", "Count", "countQuery call failed", entities.Error); TotalAPICallsMadeInCurrentSession++; }
            return entities;
        }

        /// <summary>
        /// Retrieve entities from Clarizen according to a certain criteria 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.entityQuery EntityQuery(Data.Queries.entityQuery request)
        {
            Data.entityQuery entities = new Data.entityQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(entities.BulkRequest);
            else { Logs.Assert(entities.IsCalledSuccessfully, "Ekin.Clarizen.API", "EntityQuery", "entityQuery call failed", entities.Error); TotalAPICallsMadeInCurrentSession++; }
            return entities;
        }

        /// <summary>
        /// Retrieve all entities from Clarizen of the given typeName 
        /// </summary>
        /// <param name="typeName">The main entity type to query (e.g. WorkItem, User etc.)</param>
        /// <param name="fields">A list of field names to retrieve</param>
        /// <returns></returns>
        public Data.entityQuery GetAllEntities(string typeName, string[] fields)
        {
            return EntityQuery(new Data.Queries.entityQuery(typeName, fields));
        }

        /// <summary>
        /// Returns the list of groups the current user is a member of 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.groupsQuery GroupsQuery(Data.Queries.groupsQuery request)
        {
            Data.groupsQuery entities = new Data.groupsQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(entities.BulkRequest);
            else { Logs.Assert(entities.IsCalledSuccessfully, "Ekin.Clarizen.API", "GroupsQuery", "groupsQuery call failed", entities.Error); TotalAPICallsMadeInCurrentSession++; }
            return entities;
        }

        /// <summary>
        /// Returns the list of groups the current user is a member of 
        /// </summary>
        /// <param name="fields">Field names to return</param>
        /// <returns></returns>
        public Data.groupsQuery GroupsQuery(string[] fields)
        {
            return GroupsQuery(new Data.Queries.groupsQuery(fields));
        }

        /// <summary>
        /// Performs a query and aggreagtes (and optionally groups) the results using one of the grouping functions (e.g. Count, Sum, etc.) 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.aggregateQuery AggregateQuery(Data.Queries.aggregateQuery request)
        {
            Data.aggregateQuery op = new Data.aggregateQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "AggregateQuery", "aggregateQuery call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Retrieve the related entities of an object from a specific Relation 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.relationQuery RelationQuery(Data.Queries.relationQuery request)
        {
            Data.relationQuery op = new Data.relationQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "RelationQuery", "relationQuery call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Returns the current user news feed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.newsFeedQuery NewsFeedQuery(Data.Queries.newsFeedQuery request)
        {
            Data.newsFeedQuery op = new Data.newsFeedQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "NewsFeedQuery", "newsFeedQuery call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Returns the current user news feed 
        /// </summary>
        /// <param name="mode">Mode of the news feed query: Following or All</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <param name="feedItemOptions"></param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public Data.newsFeedQuery NewsFeedQuery(newsFeedMode mode, string[] fields, string[] feedItemOptions, paging paging)
        {
            return NewsFeedQuery(new Data.Queries.newsFeedQuery(mode, fields, feedItemOptions, paging));
        }

        /// <summary>
        /// Returns the current user news feed 
        /// </summary>
        /// <param name="mode">Mode of the news feed query: Following or All</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <returns></returns>
        public Data.newsFeedQuery NewsFeedQuery(newsFeedMode mode, string[] fields)
        {
            return NewsFeedQuery(new Data.Queries.newsFeedQuery(mode, fields));
        }

        /// <summary>
        /// Returns the social feed of an object 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.entityFeedQuery EntityFeedQuery(Data.Queries.entityFeedQuery request)
        {
            Data.entityFeedQuery op = new Data.entityFeedQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "EntityFeedQuery", "entityFeedQuery call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Returns the social feed of an object 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <param name="feedItemOptions"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Data.entityFeedQuery EntityFeedQuery(string entityId, string[] fields, string[] feedItemOptions, paging paging)
        {
            return EntityFeedQuery(new Data.Queries.entityFeedQuery(entityId, fields, feedItemOptions, paging));
        }

        /// <summary>
        /// Returns the social feed of an object 
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <returns></returns>
        public Data.entityFeedQuery EntityFeedQuery(string entityId, string[] fields)
        {
            return EntityFeedQuery(new Data.Queries.entityFeedQuery(entityId, fields));
        }

        /// <summary>
        /// Retrieves the reply feed of a discussion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.repliesQuery RepliesQuery(Data.Queries.repliesQuery request)
        {
            Data.repliesQuery op = new Data.repliesQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "RepliesQuery", "repliesQuery call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Retrieves the reply feed of a discussion
        /// </summary>
        /// <param name="postId">Id of the discussion post</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <param name="feedItemOptions"></param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public Data.repliesQuery RepliesQuery(string postId, string[] fields, string[] feedItemOptions, paging paging)
        {
            return RepliesQuery(new Data.Queries.repliesQuery(postId, fields, feedItemOptions, paging));
        }

        /// <summary>
        /// Retrieves the reply feed of a discussion
        /// </summary>
        /// <param name="postId">Id of the discussion post</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <returns></returns>
        public Data.repliesQuery RepliesQuery(string postId, string[] fields)
        {
            return RepliesQuery(new Data.Queries.repliesQuery(postId, fields));
        }

        /// <summary>
        /// Retrieves expenses for a specific Project or Customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.expenseQuery ExpenseQuery(Data.Queries.expenseQuery request)
        {
            Data.expenseQuery op = new Data.expenseQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "ExpenseQuery", "expenseQuery call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        public Data.timesheetQuery TimesheetQuery(Data.Queries.timesheetQuery request)
        {
            Data.timesheetQuery op = new Data.timesheetQuery(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "TimesheetQuery", "timesheetQuery call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        #endregion

        #region Data - Other methods

        /// <summary>
        /// Performs a text search in a specific entity type or in all entity types 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.search Search(Data.Request.search request)
        {
            Data.search entities = new Data.search(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(entities.BulkRequest);
            else { Logs.Assert(entities.IsCalledSuccessfully, "Ekin.Clarizen.API", "Search", "search call failed", entities.Error); TotalAPICallsMadeInCurrentSession++; }
            return entities;
        }

        /// <summary>
        /// Performs a text search in all entity types 
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <returns></returns>
        public Data.search Search(string q)
        {
            return Search(new Data.Request.search(q));
        }

        /// <summary>
        /// Performs a text search in all entity types 
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public Data.search Search(string q, paging paging)
        {
            return Search(new Data.Request.search(q, paging));
        }

        /// <summary>
        /// Performs a text search in a specific entity type 
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <param name="typeName">The Entity Type to search. If omitted, search on all types</param>
        /// <param name="fields">The list of fields to return. Only valid when specifying a TypeName. When searching in all types the fields returned are fixed</param>
        /// <returns></returns>
        public Data.search Search(string q, string typeName, string[] fields)
        {
            return Search(new Data.Request.search(q, typeName, fields));
        }

        /// <summary>
        /// Performs a text search in a specific entity type 
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <param name="typeName">The Entity Type to search. If omitted, search on all types</param>
        /// <param name="fields">The list of fields to return. Only valid when specifying a TypeName. When searching in all types the fields returned are fixed</param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public Data.search Search(string q, string typeName, string[] fields, paging paging)
        {
            return Search(new Data.Request.search(q, typeName, fields, paging));
        }

        /// <summary>
        /// Creates a discussion message, link it related entities and notify users or groups 
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <param name="relatedEntities">Entity Ids</param>
        /// <param name="notify">Entity Ids for users or groups to notify</param>
        /// <param name="topics">Entity Ids</param>
        /// <returns></returns>
        public Data.createDiscussion CreateDiscussion(object entity, string[] relatedEntities, string[] notify, string[] topics)
        {
            return CreateDiscussion(new Data.Request.createDiscussion(entity, relatedEntities, notify, topics));
        }

        /// <summary>
        /// Creates a discussion message, link it related entities and notify users or groups 
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <param name="relatedEntities">Entity Ids</param>
        /// <param name="notify">Entity Ids for users or groups to notify</param>
        /// <returns></returns>
        public Data.createDiscussion CreateDiscussion(object entity, string[] relatedEntities, string[] notify)
        {
            return CreateDiscussion(new Data.Request.createDiscussion(entity, relatedEntities, notify));
        }

        /// <summary>
        /// Creates a discussion message and link it related entities 
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <param name="relatedEntities">Entity Ids</param>
        /// <returns></returns>
        public Data.createDiscussion CreateDiscussion(object entity, string[] relatedEntities)
        {
            return CreateDiscussion(new Data.Request.createDiscussion(entity, relatedEntities));
        }

        /// <summary>
        /// Creates a discussion message
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <returns></returns>
        public Data.createDiscussion CreateDiscussion(object entity)
        {
            return CreateDiscussion(new Data.Request.createDiscussion(entity));
        }

        /// <summary>
        /// Creates a discussion message, link it related entities and notify users or groups 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.createDiscussion CreateDiscussion(Data.Request.createDiscussion request)
        {
            Data.createDiscussion op = new Data.createDiscussion(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "CreateDiscussion", "createDiscussion call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Performs life cycle operations (Activate, Cancel etc.) on an entity 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.lifecycle Lifecycle(Data.Request.lifecycle request)
        {
            Data.lifecycle op = new Data.lifecycle(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "Lifecycle", "lifecycle call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Performs life cycle operations (Activate, Cancel etc.) on an entity 
        /// </summary>
        /// <param name="ids">A list of objects (Entity Ids) to perform the operation on</param>
        /// <param name="operation">The operation to perform ('Activate', 'Cancel' etc.)</param>
        /// <returns></returns>
        public Data.lifecycle Lifecycle(string[] ids, string operation)
        {
            return Lifecycle(new Data.Request.lifecycle(ids, operation));
        }

        /// <summary>
        /// Changes the state of an object
        /// </summary>
        /// <param name="ids">List of objects to perform the operation on</param>
        /// <param name="state">The new state</param>
        /// <returns></returns>
        public Data.changeState ChangeState(string[] ids, string state)
        {
            Data.changeState objects = new Data.changeState(new Data.Request.changeState(ids, state), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "ChangeState", "changeState call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Executes custom action
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.executeCustomAction ExecuteCustomAction(Data.Request.executeCustomAction request)
        {
            Data.executeCustomAction objects = new Data.executeCustomAction(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(objects.BulkRequest);
            else { Logs.Assert(objects.IsCalledSuccessfully, "Ekin.Clarizen.API", "ExecuteCustomAction", "executeCustomAction call failed", objects.Error); TotalAPICallsMadeInCurrentSession++; }
            return objects;
        }

        /// <summary>
        /// Executes custom action
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="customAction"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public Data.executeCustomAction ExecuteCustomAction(string targetId, string customAction, fieldValue[] values)
        {
            return ExecuteCustomAction(new Data.Request.executeCustomAction(targetId, customAction, values));
        }

        /// <summary>
        /// Returns the list of template available for a certain Entity Type 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Data.getTemplateDescriptions GetTemplateDescriptions(string typeName)
        {
            Data.getTemplateDescriptions op = new Data.getTemplateDescriptions(new Data.Request.getTemplateDescriptions(typeName), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetTemplateDescriptions", "getTemplateDescriptions call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Creates an entity from a predefined template 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Data.createFromTemplate CreateFromTemplate(object entity, string templateName, string parentId)
        {
            Data.createFromTemplate op = new Data.createFromTemplate(new Data.Request.createFromTemplate(entity, templateName, parentId), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "CreateFromTemplate", "createFromTemplate call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Provides information about calendar definitions in Clarizen. This API can provide information about the organization calendar or a user calendar 
        /// </summary>
        /// <param name="id">The id of the entity (organization or user) to get the calendar info for</param>
        /// <returns></returns>
        public Data.getCalendarInfo GetCalendarInfo(string id)
        {
            Data.getCalendarInfo op = new Data.getCalendarInfo(new Data.Request.getCalendarInfo(id), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetCalendarInfo", "getCalendarInfo call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        public Data.getCalendarExceptions GetCalendarExceptions(string id, DateTime fromDate, DateTime toDate)
        {
            Data.getCalendarExceptions op = new Data.getCalendarExceptions(new Data.Request.getCalendarExceptions(id, fromDate, toDate), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetCalendarExceptions", "GetCalendarExceptions call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Returns the missing timesheets of a user between given dates
        /// </summary>
        /// <param name="user">EntityId of the User. If querying a multi-instance environment use /Organization/orgId/User/userId</param>
        /// <param name="startDate">The start of the date range (inclusive)</param>
        /// <param name="endDate">The end of the date range (exclusive)</param>
        /// <returns></returns>
        public Data.getMissingTimesheets GetMissingTimesheets(string user, DateTime startDate, DateTime endDate)
        {
            Data.getMissingTimesheets op = new Data.getMissingTimesheets(new Data.Request.getMissingTimesheets(user, startDate, endDate), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetMissingTimesheets", "GetMissingTimesheets call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        #endregion

        #region CZQL

        /// <summary>
        /// Executes a Clarizen Query Language (CZQL) query. Visit https://api.clarizen.com/V2.0/services/data/Query for more information.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Data.query ExecuteQuery(Data.Request.query query)
        {
            Data.query CZQuery = new Data.query(query, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(CZQuery.BulkRequest);
            else { Logs.Assert(CZQuery.IsCalledSuccessfully, "Ekin.Clarizen.API", "ExecuteQuery", "query call failed", CZQuery.Error); TotalAPICallsMadeInCurrentSession++; }
            return CZQuery;
        }

        /// <summary>
        /// Executes a Clarizen Query Language (CZQL) query. Visit https://api.clarizen.com/V2.0/services/data/Query for more information.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Data.query ExecuteQuery(Interfaces.IClarizenQuery query)
        {
            return ExecuteQuery(new Data.Request.query(query.ToCZQL()));
        }

        #endregion

        #region Bulk Service

        /// <summary>
        /// Starts the bulk service. API calls are not executed until CommitBulkService function is called.
        /// </summary>
        public void StartBulkService()
        {
            isBulk = true;
            bulkRequests = new List<request> { };
        }

        /// <summary>
        /// Executes all the calls in the buffer in a single Clarizen bulk API call.
        /// </summary>
        /// <param name="transactional"></param>
        /// <param name="batch"></param>
        /// <param name="includeRequestsInResponses">Embed requests in responses so that when there is an error in a bulk operation you can look into the request that caused it</param>
        /// <returns></returns>
        public Bulk.execute CommitBulkService(bool transactional = false, bool? batch = null, bool? includeRequestsInResponses = null, int? timeout = null)
        {
            Logs.Assert(isBulk, "Ekin.Clarizen.API", "CommitBulkService", "Bulk service not started");
            if (isBulk)
            {
                CallSettings callSettings = CallSettings.GetFromAPI(this);
                callSettings.timeout = timeout;
                Bulk.execute bulkService = new Bulk.execute(new Bulk.Request.execute(bulkRequests, transactional, batch), callSettings);
                Logs.Assert(bulkService.IsCalledSuccessfully, "Ekin.Clarizen.API", "CommitBulkService", "Bulk service error", bulkService.Error);
                TotalAPICallsMadeInCurrentSession++;
                if (bulkService.IsCalledSuccessfully)
                {
                    if (bulkService.Data.responses?.Length > 0)
                    {
                        for (int n = 0; n < bulkService.Data.responses.Length; n++)
                        {
                            if (bulkService.Data.responses[n].statusCode == 200)
                                bulkService.Data.responses[n].CastBody(bulkRequests[n].resultType);
                            else
                                bulkService.Data.responses[n].CastBodyToError();

                            if (includeRequestsInResponses.GetValueOrDefault(false))
                            {
                                // For every request in the payload Clarizen returns a response so their indexes must match
                                bulkService.Data.responses[n].request = bulkRequests[n];
                            }
                        }
                    }
                    else
                    {
                        Logs.AddError("Ekin.Clarizen.API", "CommitBulkService", "Bulk service executed successfully but no response was received from Clarizen");
                    }
                }
                return bulkService;
            }
            return null;
        }

        /// <summary>
        /// Deletes the current Bulk buffer
        /// </summary>
        public void CancelBulkService()
        {
            isBulk = false;
            bulkRequests = new List<request> { };
        }

        #endregion

        #region Utils

        /// <summary>
        /// Converts an API session to a web application session. Calling this method returns a url to the application that will not require credentials to login
        /// </summary>
        /// <returns></returns>
        public Utils.appLogin AppLogin()
        {
            Utils.appLogin util = new Utils.appLogin(CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(util.BulkRequest);
            else { Logs.Assert(util.IsCalledSuccessfully, "Ekin.Clarizen.API", "AppLogin", "appLogin call failed", util.Error); TotalAPICallsMadeInCurrentSession++; }
            return util;
        }

        /// <summary>
        /// Send an email and attach it to an object in Clarizen
        /// </summary>
        /// <param name="recipients">To/CC emails and Entity Ids for users</param>
        /// <param name="subject">Subject line of the email to be sent</param>
        /// <param name="body">Body of the email to be sent</param>
        /// <param name="relatedEntityId">Entity Id of the related object</param>
        /// <param name="accessType">Public or Private</param>
        /// <returns></returns>
        public Utils.sendEMail SendEmail(recipient[] recipients, string subject, string body, string relatedEntityId, Utils.Request.sendEMail.CZAccessType accessType)
        {
            Utils.sendEMail util = new Utils.sendEMail(new Utils.Request.sendEMail(recipients, subject, body, relatedEntityId, accessType), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(util.BulkRequest);
            else { Logs.Assert(util.IsCalledSuccessfully, "Ekin.Clarizen.API", "SendEmail", "sendEMail call failed", util.Error); TotalAPICallsMadeInCurrentSession++; }
            return util;
        }

        #endregion

        #region Applications

        public Applications.getApplicationStatus GetApplicationStatus(string applicationId)
        {
            Applications.getApplicationStatus op = new Applications.getApplicationStatus(new Applications.Request.getApplicationStatus(applicationId), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetApplicationStatus", "getApplicationStatus call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        public Applications.installApplication InstallApplication(string applicationId, bool autoEnable)
        {
            Applications.installApplication op = new Applications.installApplication(new Applications.Request.installApplication(applicationId, autoEnable), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "InstallApplication", "installApplication call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        #endregion

        #region Files

        /// <summary>
        /// Gets download information about a file attached to a document 
        /// </summary>
        /// <param name="documentId">Entity Id</param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        public Files.download Download(string documentId, bool redirect)
        {
            Files.download op = new Files.download(new Files.Request.download(documentId, redirect), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "Download", "download call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Get a URL for uploading files. After POSTing a file to this URL, perform an Upload operation and pass this URL in the UploadUrl parameter. 
        /// </summary>
        /// <returns></returns>
        public Files.getUploadUrl GetUploadUrl()
        {
            Files.getUploadUrl op = new Files.getUploadUrl(CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "GetUploadUrl", "getUploadUrl call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Upload file to a document in Clarizen 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Files.upload Upload(Files.Request.upload request)
        {
            Files.upload op = new Files.upload(request, CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "Upload", "upload call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        /// <summary>
        /// Upload file to a document in Clarizen
        /// </summary>
        /// <param name="documentId">Id of a document to attach to</param>
        /// <param name="fileInformation">Additional information about the file</param>
        /// <param name="uploadUrl">When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl</param>
        /// <returns></returns>
        public Files.upload Upload(string documentId, fileInformation fileInformation, string uploadUrl)
        {
            return Upload(new Files.Request.upload(documentId, fileInformation, uploadUrl));
        }

        /// <summary>
        /// Upload file to a document in Clarizen
        /// </summary>
        /// <returns></returns>
        public Files.upload Upload(string documentId, storageType storage, string url, string fileName, string subType, string extendedInfo, string uploadUrl)
        {
            return Upload(new Files.Request.upload(documentId, new fileInformation(storage, url, fileName, subType, extendedInfo), uploadUrl));
        }

        /// <summary>
        /// Set (Or Reset) the image of an object in Clarizen 
        /// </summary>
        /// <param name="entityId">Id of an entity to attach to</param>
        /// <param name="uploadUrl">When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl</param>
        /// <param name="reset">Revert image to default icon</param>
        /// <returns></returns>
        public Files.updateImage UpdateImage(string entityId, string uploadUrl, bool reset)
        {
            Files.updateImage op = new Files.updateImage(new Files.Request.updateImage(entityId, uploadUrl, reset), CallSettings.GetFromAPI(this));
            if (isBulk) bulkRequests.Add(op.BulkRequest);
            else { Logs.Assert(op.IsCalledSuccessfully, "Ekin.Clarizen.API", "UpdateImage", "updateImage call failed", op.Error); TotalAPICallsMadeInCurrentSession++; }
            return op;
        }

        //
        #endregion

    }
}