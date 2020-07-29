using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ekin.Clarizen.Authentication;
using Ekin.Clarizen.Interfaces;
using Ekin.Log;
using Newtonsoft.Json.Linq;

namespace Ekin.Clarizen
{
    /// <summary>
    /// .Net wrapper for the Clarizen API v2.0 located at https://api.clarizen.com/V2.0/services
    /// Developed by Ekin Caglar - ekin@caglar.com - in October 2016
    /// Contributors since then (with special thanks): Mustafa Kipergil, Roberto Rey Linares, Zoya Feofanova
    /// </summary>
    public class API
    {
        #region Private Properties
        internal string Requester { get; set; }
        private BulkOperations _bulk { get; set; }
        private FileUploadHelper _fileUpload { get; set; }

        #endregion

        #region Public properties

        public bool RemoveInvalidFieldsFromJsonResult { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ServerLocation { get; set; }
        public string SessionId { get; set; }

        public string ApiKey { get; set; }
        public string Redirect { get; set; }

        public bool IsSandbox { get; set; } = false;
        public int TotalAPICallsMadeInCurrentSession { get; set; }
        public bool SerializeNullValues { get; set; } = false;
        public int Retry { get; set; } = 1;
        public int SleepBetweenRetries { get; set; } = 0;
        public int? ConnectionLimit { get; set; } = null;

        public LogFactory Logs { get; set; }

        public BulkOperations Bulk
        {
            get
            {
                if (_bulk == null)
                {
                    _bulk = new BulkOperations(this);
                }
                return _bulk;
            }
        }
        public FileUploadHelper FileUpload
        {
            get
            {
                if (_fileUpload == null)
                {
                    _fileUpload = new FileUploadHelper(this);
                }
                return _fileUpload;
            }
        }

        public bool IsBulk { get; private set; }
        private List<Request> BulkRequests { get; set; }

        public int? Timeout { get; set; } = 120000;

        #endregion Public properties

        #region Events

        public event EventHandler<SessionRefreshedEventArgs> SessionRefreshed;
        public event EventHandler SessionExpired;

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
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            System.Net.ServicePointManager.Expect100Continue = false;
        }

        /// <summary>
        /// When the API class is initiated as static, to use Bulk queries you need to clone it after calling Login().
        /// </summary>
        /// <returns></returns>
        public API Clone()
        {
            return new API()
            {
                SessionId = this.SessionId,
                ApiKey = this.ApiKey,
                IsSandbox = this.IsSandbox,
                ServerLocation = this.ServerLocation,
                Username = this.Username,
                Password = this.Password,
                Retry = this.Retry,
                SleepBetweenRetries = this.SleepBetweenRetries,
                SerializeNullValues = this.SerializeNullValues,
                Timeout = this.Timeout,
                RemoveInvalidFieldsFromJsonResult = this.RemoveInvalidFieldsFromJsonResult,
                Redirect = this.Redirect,
                ConnectionLimit = this.ConnectionLimit
                // We don't copy Logs, Bulk and Requester in the clone
            };
        }

        public void SetRequester(string value)
        {
            Requester = value;
            if (Bulk?.ClarizenAPI != null)
            {
                Bulk.ClarizenAPI.Requester = value;
            }
            if (FileUpload?.ClarizenAPI != null)
            {
                FileUpload.ClarizenAPI.Requester = value;
            }
        }

        #region Event handlers and delegates

        protected virtual void OnSessionRefreshed(SessionRefreshedEventArgs e)
        {
            SessionRefreshed?.Invoke(this, e);
        }

        protected virtual void OnSessionExpired(EventArgs e)
        {
            SessionExpired?.Invoke(this, e);
        }

        private static void Call_SessionTimeout(object sender, EventArgs e)
        {
            if (sender is API)
            {
                API ClarizenAPI = (API)sender;
                if (ClarizenAPI.Login().Result)
                {
                    ClarizenAPI.OnSessionRefreshed(new SessionRefreshedEventArgs { NewSessionId = ClarizenAPI.SessionId });
                }
                else
                {
                    ClarizenAPI.Logs.AddError("API", "Call_SessionTimeout", "Session has expired but could not be refreshed.");
                    ClarizenAPI.OnSessionExpired(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Authentication methods

        /// <summary>
        /// Opens a new session using the credentials given for accessing Clarizen API
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password)
        {
            Username = username;
            Password = password;
            return await Login();
        }

        /// <summary>
        /// Opens a new session using the credentials given for accessing Clarizen API
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Login()
        {
            if (string.IsNullOrWhiteSpace(ServerLocation))
            {
                // First we get the Url where your organization API is located
                Authentication.GetServerDefinition serverDefinition = new Authentication.GetServerDefinition(new Ekin.Clarizen.Authentication.Request.GetServerDefinition(Username, Password, new Ekin.Clarizen.Authentication.Request.LoginOptions()), IsSandbox);
                bool executionResult = await serverDefinition.Execute();
                //TotalAPICallsMadeInCurrentSession++; // Login call doesn't count towards quota (Confirmed by Yaron Perlman on 9 Nov 2016)
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "Login", "Server definition could not be retrieved", serverDefinition.Error);
                if (executionResult)
                {
                    ServerLocation = serverDefinition.Data.ServerLocation;
                }
            }

            if (!string.IsNullOrWhiteSpace(ServerLocation))
            {
                // Then we login to the API at the above location
                Authentication.Login apiCall = new Authentication.Login(ServerLocation, new Ekin.Clarizen.Authentication.Request.Login(Username, Password, new Ekin.Clarizen.Authentication.Request.LoginOptions()));
                bool executionResult = await apiCall.Execute();
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "Login", $"Login failed for {Username}", apiCall.Error);
                if (executionResult)
                {
                    // Upon successful login a unique ID representing the current session is returned.
                    // This ID is then passed on to all the API calls in the Http Header
                    SessionId = apiCall.Data.SessionId;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Loges the user out and closes the current session
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Logout()
        {
            Authentication.Logout apiCall = new Authentication.Logout(ServerLocation, SessionId);
            bool executionResult = await apiCall.Execute();
            //TotalAPICallsMadeInCurrentSession++; // Logout call doesn't count towards quota (Confirmed by Yaron Perlman on 9 Nov 2016)
            Logs.Assert(executionResult, "Ekin.Clarizen.API", "Logout", "Logout failed");
            return (executionResult);
        }

        /// <summary>
        /// In a multi-org environment this endpoint allows administrators at the master org to set a password for any user in the system. It changes passwords without sending any email.
        /// </summary>
        /// <param name="userId">Fully qualified Id of the user. Example: /Organization/SomeOrgId/User/SomeUserId</param>
        /// <param name="newPassword">New password for the user. The Password will be checked for complexity and for history. The user will not be required to change the password after login.</param>
        /// <returns></returns>
        public async Task<bool> SetPassword(string userId, string newPassword)
        {
            Authentication.SetPassword apiCall = new Authentication.SetPassword(new Authentication.Request.SetPassword(userId, newPassword), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            Logs.Assert(executionResult, "Ekin.Clarizen.API", "setPassword", apiCall.Error);
            return (executionResult);
        }

        /// <summary>
        /// Returns information about the current session
        /// </summary>
        /// <returns></returns>
        public async Task<Authentication.GetSessionInfo> GetSessionInfo()
        {
            Authentication.GetSessionInfo apiCall = new Authentication.GetSessionInfo(ServerLocation, SessionId);
            bool executionResult = await apiCall.Execute();
            TotalAPICallsMadeInCurrentSession++;
            Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetSessionInfo", "Session info could be retrieved", apiCall.Error);
            return apiCall;
        }

        #endregion Authentication methods

        #region Metadata methods

        /// <summary>
        /// Returns information about the entity types in your organization
        /// </summary>
        /// <param name="typeNames"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public async Task<Metadata.DescribeMetadata> DescribeMetadata(string[] typeNames, string[] flags)
        {
            Metadata.DescribeMetadata apiCall = new Metadata.DescribeMetadata(
                ((typeNames == null && flags == null) ? new Ekin.Clarizen.Metadata.Request.DescribeMetadata() :
                                                        new Ekin.Clarizen.Metadata.Request.DescribeMetadata(typeNames, flags)),
                CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else
            {
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "DescribeMetadata", "describeMetadata failed", apiCall.Error);
                TotalAPICallsMadeInCurrentSession++;
            }
            return apiCall;
        }

        /// <summary>
        /// Returns information about the entity types in your organization
        /// </summary>
        /// <param name="typeNames"></param>
        /// <returns></returns>
        public async Task<Metadata.DescribeMetadata> DescribeMetadata(string[] typeNames)
        {
            return await DescribeMetadata(typeNames, null);
        }

        /// <summary>
        /// Returns information about the entity types in your organization
        /// </summary>
        /// <returns></returns>
        public async Task<Metadata.DescribeMetadata> DescribeMetadata()
        {
            return await DescribeMetadata(null, null);
        }

        /// <summary>
        /// Returns the list of entity types available for your organization
        /// </summary>
        /// <returns></returns>
        public async Task<Metadata.ListEntities> ListEntities()
        {
            Metadata.ListEntities apiCall = new Metadata.ListEntities(CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "ListEntities", "listEntities failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Returns information about an Entity Type in Clarizen, including its fields, relations and states
        /// </summary>
        /// <param name="typeNames"></param>
        /// <returns></returns>
        public async Task<Metadata.DescribeEntities> DescribeEntities(string[] typeNames)
        {
            Metadata.DescribeEntities apiCall = new Metadata.DescribeEntities(new Metadata.Request.DescribeEntities(typeNames), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "DescribeEntities", "describeEntities failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Describes the relation between entities
        /// </summary>
        /// <param name="typeNames"></param>
        /// <returns></returns>
        public async Task<Metadata.DescribeEntityRelations> DescribeEntityRelations(string[] typeNames)
        {
            Metadata.DescribeEntityRelations apiCall = new Metadata.DescribeEntityRelations(new Metadata.Request.DescribeEntityRelations(typeNames), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "DescribeEntityRelations", "describeEntityRelations failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Create a workflow rule in Clarizen.
        /// </summary>
        public async Task<Metadata.Objects_put> CreateWorkflowRule(string forType, string name, string description, string triggerType, string criteria, string action_url, string action_method, string action_headers, string action_body)
        {
            return await CreateWorkflowRule(new Metadata.Request.Objects_put(forType, name, description, triggerType, criteria, new Action(action_url, action_method, action_headers, action_body)));
        }

        /// <summary>
        /// Create a workflow rule in Clarizen.
        /// </summary>
        public async Task<Metadata.Objects_put> CreateWorkflowRule(string forType, string name, string description, string triggerType, string criteria, Action action)
        {
            return await CreateWorkflowRule(new Metadata.Request.Objects_put(forType, name, description, triggerType, criteria, action));
        }

        /// <summary>
        /// Create a workflow rule in Clarizen.
        /// </summary>
        public async Task<Metadata.Objects_put> CreateWorkflowRule(Metadata.Request.Objects_put request)
        {
            Metadata.Objects_put apiCall = new Metadata.Objects_put(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "CreateWorkflowRule", "CreateWorkflowRule call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Delete an workflow rule in Clarizen
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Metadata.Objects_delete> DeleteWorkflowRule(string id)
        {
            Metadata.Objects_delete apiCall = new Metadata.Objects_delete(new Metadata.Request.Objects_delete(id), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "DeleteWorkflowRule", "DeleteWorkflowRule call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieves the values of a system settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<Metadata.GetSystemSettingsValues> GetSystemSettingsValues(string[] settings)
        {
            Metadata.GetSystemSettingsValues apiCall = new Metadata.GetSystemSettingsValues(new Metadata.Request.GetSystemSettingsValues(settings), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetSystemSettingsValues", "getSystemSettingsValues failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Save the values of a system settings
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<Metadata.SetSystemSettingsValues> SetSystemSettingsValues(FieldValue[] settings)
        {
            Metadata.SetSystemSettingsValues apiCall = new Metadata.SetSystemSettingsValues(new Metadata.Request.SetSystemSettingsValues(settings), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "SetSystemSettingsValues", "setSystemSettingsValues call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        #endregion Metadata methods

        #region Data - CRUD methods

        /// <summary>
        /// Read an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to get</param>
        /// <param name="fields">The list of fields to read</param>
        /// <returns></returns>
        public async Task<Data.Objects_get> GetObject(string id, string[] fields)
        {
            Data.Objects_get apiCall = new Data.Objects_get(new Data.Request.Objects_get(id, fields), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetObject", "objects_get call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Read an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to get</param>
        /// <returns></returns>
        public async Task<Data.Objects_get> GetObject(string id)
        {
            return await GetObject(id: id, fields: null);
        }

        /// <summary>
        /// Read an entity in Clarizen
        /// </summary>
        /// <typeparam name="T">Return type of the operation</typeparam>
        /// <param name="id">Entity Id of the object to get</param>
        /// <returns></returns>
        public async Task<T> GetObject<T>(string id)
        {
            string[] fields = typeof(T).GetPropertyList();
            Data.Objects_get apiCall = new Data.Objects_get(new Data.Request.Objects_get(id, fields), CallSettings.GetFromAPI(this), true);
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetObject", "objects_get call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            if (executionResult)
            {
                try
                {
                    JObject obj = JObject.Parse(apiCall.Data);
                    RemoveInvalidFields(obj);
                    return obj.ToObject<T>();
                }
                catch (Exception ex)
                {
                    Logs.AddError("Ekin.Clarizen.API", "GetObject", ex);
                    return default;
                }
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Read an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to get</param>
        /// <param name="pocoObject">Return type of the operation</param>
        /// <returns></returns>
        public async Task<dynamic> GetObject(string id, Type pocoObject)
        {
            string[] fields = pocoObject.GetPropertyList();
            Data.Objects_get apiCall = new Data.Objects_get(new Data.Request.Objects_get(id, fields), CallSettings.GetFromAPI(this), true);
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetObject", "objects_get call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            if (executionResult)
            {
                try
                {
                    if (apiCall.Data != null)
                    {
                        JObject obj = JObject.Parse(apiCall.Data);
                        RemoveInvalidFields(obj);
                        return obj.ToObject(pocoObject);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Logs.AddError("Ekin.Clarizen.API", "GetObject", ex);
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
        public async Task<Data.Objects_put> CreateObject(string id, object obj)
        {
            Data.Objects_put apiCall = new Data.Objects_put(id, obj, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "CreateObject", "objects_put call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Update an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to update</param>
        /// <param name="obj">Object to update</param>
        /// <returns></returns>
        public async Task<Data.Objects_post> UpdateObject(string id, object obj)
        {
            Data.Objects_post apiCall = new Data.Objects_post(id, obj, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "UpdateObject", "objects_post call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Delete an entity in Clarizen
        /// </summary>
        /// <param name="id">Entity Id of the object to delete</param>
        /// <returns></returns>
        public async Task<Data.Objects_delete> DeleteObject(string id)
        {
            Data.Objects_delete apiCall = new Data.Objects_delete(new Ekin.Clarizen.Data.Request.Objects_delete(id), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "DeleteObject", "objects_delete call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Creates and immediatly retrieves an entity
        /// </summary>
        /// <param name="entity">Entity to be created. Should include Id field such as /Task</param>
        /// <param name="fields">List of fields to retrieve</param>
        /// <returns></returns>
        public async Task<Data.CreateAndRetrieve> CreateAndRetrieve(object entity, string[] fields)
        {
            Data.CreateAndRetrieve apiCall = new Data.CreateAndRetrieve(new Data.Request.CreateAndRetrieve(entity, fields), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "CreateAndRetrieve", "createAndRetrieve call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieves multiple entities from the same entity type in a single request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.RetrieveMultiple> RetrieveMultiple(Data.Request.RetrieveMultiple request)
        {
            Data.RetrieveMultiple apiCall = new Data.RetrieveMultiple(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "RetrieveMultiple", "retrieveMultiple call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieves multiple entities from the same entity type in a single request
        /// </summary>
        /// <param name="fields">Fields to retrieve</param>
        /// <param name="ids">Entity Ids</param>
        /// <returns></returns>
        public async Task<Data.RetrieveMultiple> RetrieveMultiple(string[] fields, string[] ids)
        {
            return await RetrieveMultiple(new Data.Request.RetrieveMultiple(fields, ids));
        }

        /// <summary>
        /// Gets all instances of an entity from Clarizen recursively
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="pocoObject"></param>
        /// <returns></returns>
        public async Task<GetAllResult> GetAll(string entityName, Type pocoObject, ICondition condition = null, int? pageSize = null, int? sleepTime = null)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { pocoObject });
            System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(listType);
            GetAllResult result = new GetAllResult()
            {
                Errors = new List<Error>() { }
            };
            Paging paging = new Paging
            {
                Limit = pageSize.GetValueOrDefault(0) > 0 ? pageSize.GetValueOrDefault(0) : 1000
            };
            bool hasMore = true;
            while (hasMore)
            {
                Data.Queries.EntityQuery request =
                    new Data.Queries.EntityQuery(entityName, pocoObject.GetPropertyList(), null, null, null, false, false, paging);
                if (condition != null)
                    request.Where = condition;
                Data.EntityQuery apiCall = await EntityQuery(request);
                apiCall.SessionTimeout += Call_SessionTimeout;
                bool executionResult = await apiCall.Execute();
                if (executionResult)
                {
                    foreach (JObject obj in apiCall.Data.Entities)
                    {
                        RemoveInvalidFields(obj);
                        list.Add(obj.ToObject(pocoObject));
                    }
                    paging = apiCall.Data.Paging;
                    hasMore = apiCall.Data.Paging.HasMore;
                }
                else
                {
                    result.Errors.Add(new Error("", "Entity query failed with error: " + apiCall.Error));
                    hasMore = false;
                }
                if (sleepTime.GetValueOrDefault() > 0)
                {
                    await Task.Delay(sleepTime.GetValueOrDefault());
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
        public async Task<GetAllResult> GetAll(Interfaces.IClarizenQuery query, Type pocoObject, int? pageSize = null, int? sleepTime = null)
        {
            Type listType = typeof(List<>).MakeGenericType(new[] { pocoObject });
            System.Collections.IList list = (System.Collections.IList)Activator.CreateInstance(listType);
            GetAllResult result = new GetAllResult()
            {
                Errors = new List<Error>() { }
            };
            Paging paging = new Paging
            {
                Limit = pageSize.GetValueOrDefault(0) > 0 ? pageSize.GetValueOrDefault(0) : 1000
            };
            bool hasMore = true;
            while (hasMore)
            {
                CallSettings callSettings = CallSettings.GetFromAPI(this);
                callSettings.IsBulk = false;
                Data.Query apiCall = new Data.Query(new Data.Request.Query(query.ToCZQL(), paging), callSettings);
                apiCall.SessionTimeout += Call_SessionTimeout;
                bool executionResult = await apiCall.Execute();
                if (executionResult)
                {
                    foreach (JObject obj in apiCall.Data.Entities)
                    {
                        RemoveInvalidFields(obj);
                        list.Add(obj.ToObject(pocoObject));
                    }
                    paging = apiCall.Data.Paging;
                    hasMore = apiCall.Data.Paging.HasMore;
                }
                else
                {
                    string message = "Query failed with error" + (callSettings.Retry > 1 ? $" after {callSettings.Retry} retries" : "") + ": " + apiCall.Error;
                    result.Errors.Add(new Error("", message));
                    hasMore = false;
                }
                if (sleepTime.GetValueOrDefault() > 0)
                {
                    await Task.Delay(sleepTime.GetValueOrDefault());
                }
            }
            result.Data = list;
            return result;
        }

        public async Task<GetAllResult> GetAllByFields(string entityName, string[] fields, ICondition condition = null, int? pageSize = null, int? sleepTime = null)
        {
            List<dynamic> list = new List<dynamic>();
            GetAllResult result = new GetAllResult()
            {
                Errors = new List<Error>() { }
            };
            Paging paging = new Paging
            {
                Limit = pageSize.GetValueOrDefault(0) > 0 ? pageSize.GetValueOrDefault(0) : 5000
            };
            bool hasMore = true;
            while (hasMore)
            {
                Ekin.Clarizen.Data.Queries.EntityQuery request = new Ekin.Clarizen.Data.Queries.EntityQuery(entityName, fields, null, null, null, false, false, paging);
                if (condition != null)
                    request.Where = condition;
                Ekin.Clarizen.Data.EntityQuery apiCall = await EntityQuery(request);
                apiCall.SessionTimeout += Call_SessionTimeout;
                bool executionResult = await apiCall.Execute();
                if (executionResult)
                {
                    foreach (JObject obj in apiCall.Data.Entities)
                    {
                        list.Add(obj);
                    }
                    paging = apiCall.Data.Paging;
                    hasMore = apiCall.Data.Paging.HasMore;
                }
                else
                {
                    result.Errors.Add(new Error("", "Entity query failed with error: " + apiCall.Error));
                    hasMore = false;
                }
                if (sleepTime.GetValueOrDefault() > 0)
                {
                    await Task.Delay(sleepTime.GetValueOrDefault());
                }
            }
            result.Data = list;
            return result;
        }

        private void RemoveInvalidFields(JObject obj)
        {
            if (RemoveInvalidFieldsFromJsonResult)
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

        #endregion Data - CRUD methods

        #region Data - Query methods

        /// <summary>
        /// Performs a query and returns the result count
        /// </summary>
        /// <param name="query">findUserQuery, entityQuery, expenseQuery, timesheetQuery, aggregateQuery, relationQuery, cZQLQuery, entityFeedQuery, groupsQuery, newsFeedQuery, repliesQuery</param>
        /// <returns></returns>
        public async Task<Data.CountQuery> Count(IQuery query)
        {
            Data.CountQuery apiCall = new Data.CountQuery(new Data.Request.CountQuery(query), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "Count", "countQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieve entities from Clarizen according to a certain criteria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.EntityQuery> EntityQuery(Data.Queries.EntityQuery request)
        {
            Data.EntityQuery apiCall = new Data.EntityQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "EntityQuery", "entityQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieve entities from multiple organizations in Clarizen for a specific user and according to a certain criteria
        /// </summary>
        public async Task<Data.CrossOrgEntityQuery> CrossOrgEntityQuery(Data.Queries.CrossOrgEntityQuery request)
        {
            Data.CrossOrgEntityQuery apiCall = new Data.CrossOrgEntityQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "CrossOrgEntityQuery", "crossOrgEntityQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieve all entities from Clarizen of the given typeName
        /// </summary>
        /// <param name="typeName">The main entity type to query (e.g. WorkItem, User etc.)</param>
        /// <param name="fields">A list of field names to retrieve</param>
        /// <returns></returns>
        public async Task<Data.EntityQuery> GetAllEntities(string typeName, string[] fields)
        {
            return await EntityQuery(new Data.Queries.EntityQuery(typeName, fields));
        }

        /// <summary>
        /// Returns the list of groups the current user is a member of
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.GroupsQuery> GroupsQuery(Data.Queries.GroupsQuery request)
        {
            Data.GroupsQuery apiCall = new Data.GroupsQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GroupsQuery", "groupsQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Returns the list of groups the current user is a member of
        /// </summary>
        /// <param name="fields">Field names to return</param>
        /// <returns></returns>
        public async Task<Data.GroupsQuery> GroupsQuery(string[] fields)
        {
            return await GroupsQuery(new Data.Queries.GroupsQuery(fields));
        }

        /// <summary>
        /// Performs a query and aggreagtes (and optionally groups) the results using one of the grouping functions (e.g. Count, Sum, etc.)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.AggregateQuery> AggregateQuery(Data.Queries.AggregateQuery request)
        {
            Data.AggregateQuery apiCall = new Data.AggregateQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "AggregateQuery", "aggregateQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieve the related entities of an object from a specific Relation
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.RelationQuery> RelationQuery(Data.Queries.RelationQuery request)
        {
            Data.RelationQuery apiCall = new Data.RelationQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "RelationQuery", "relationQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Returns the current user news feed
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.NewsFeedQuery> NewsFeedQuery(Data.Queries.NewsFeedQuery request)
        {
            Data.NewsFeedQuery apiCall = new Data.NewsFeedQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "NewsFeedQuery", "newsFeedQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Returns the current user news feed
        /// </summary>
        /// <param name="mode">Mode of the news feed query: Following or All</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <param name="feedItemOptions"></param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public async Task<Data.NewsFeedQuery> NewsFeedQuery(NewsFeedMode mode, string[] fields, string[] feedItemOptions, Paging paging)
        {
            return await NewsFeedQuery(new Data.Queries.NewsFeedQuery(mode, fields, feedItemOptions, paging));
        }

        /// <summary>
        /// Returns the current user news feed
        /// </summary>
        /// <param name="mode">Mode of the news feed query: Following or All</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <returns></returns>
        public async Task<Data.NewsFeedQuery> NewsFeedQuery(NewsFeedMode mode, string[] fields)
        {
            return await NewsFeedQuery(new Data.Queries.NewsFeedQuery(mode, fields));
        }

        /// <summary>
        /// Returns the social feed of an object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.EntityFeedQuery> EntityFeedQuery(Data.Queries.EntityFeedQuery request)
        {
            Data.EntityFeedQuery apiCall = new Data.EntityFeedQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "EntityFeedQuery", "entityFeedQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Returns the social feed of an object
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <param name="feedItemOptions"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<Data.EntityFeedQuery> EntityFeedQuery(string entityId, string[] fields, string[] feedItemOptions, Paging paging)
        {
            return await EntityFeedQuery(new Data.Queries.EntityFeedQuery(entityId, fields, feedItemOptions, paging));
        }

        /// <summary>
        /// Returns the social feed of an object
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <returns></returns>
        public async Task<Data.EntityFeedQuery> EntityFeedQuery(string entityId, string[] fields)
        {
            return await EntityFeedQuery(new Data.Queries.EntityFeedQuery(entityId, fields));
        }

        /// <summary>
        /// Retrieves the reply feed of a discussion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.RepliesQuery> RepliesQuery(Data.Queries.RepliesQuery request)
        {
            Data.RepliesQuery apiCall = new Data.RepliesQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "RepliesQuery", "repliesQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Retrieves the reply feed of a discussion
        /// </summary>
        /// <param name="postId">Id of the discussion post</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <param name="feedItemOptions"></param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public async Task<Data.RepliesQuery> RepliesQuery(string postId, string[] fields, string[] feedItemOptions, Paging paging)
        {
            return await RepliesQuery(new Data.Queries.RepliesQuery(postId, fields, feedItemOptions, paging));
        }

        /// <summary>
        /// Retrieves the reply feed of a discussion
        /// </summary>
        /// <param name="postId">Id of the discussion post</param>
        /// <param name="fields">List of Fields the query should return</param>
        /// <returns></returns>
        public async Task<Data.RepliesQuery> RepliesQuery(string postId, string[] fields)
        {
            return await RepliesQuery(new Data.Queries.RepliesQuery(postId, fields));
        }

        /// <summary>
        /// Retrieves expenses for a specific Project or Customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.ExpenseQuery> ExpenseQuery(Data.Queries.ExpenseQuery request)
        {
            Data.ExpenseQuery apiCall = new Data.ExpenseQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "ExpenseQuery", "expenseQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        public async Task<Data.TimesheetQuery> TimesheetQuery(Data.Queries.TimesheetQuery request)
        {
            Data.TimesheetQuery apiCall = new Data.TimesheetQuery(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "TimesheetQuery", "timesheetQuery call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        #endregion Data - Query methods

        #region Data - Other methods

        /// <summary>
        /// Performs a text search in a specific entity type or in all entity types
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.Search> Search(Data.Request.Search request)
        {
            Data.Search apiCall = new Data.Search(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "Search", "search call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Performs a text search in all entity types
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <returns></returns>
        public async Task<Data.Search> Search(string q)
        {
            return await Search(new Data.Request.Search(q));
        }

        /// <summary>
        /// Performs a text search in all entity types
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public async Task<Data.Search> Search(string q, Paging paging)
        {
            return await Search(new Data.Request.Search(q, paging));
        }

        /// <summary>
        /// Performs a text search in a specific entity type
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <param name="typeName">The Entity Type to search. If omitted, search on all types</param>
        /// <param name="fields">The list of fields to return. Only valid when specifying a TypeName. When searching in all types the fields returned are fixed</param>
        /// <returns></returns>
        public async Task<Data.Search> Search(string q, string typeName, string[] fields)
        {
            return await Search(new Data.Request.Search(q, typeName, fields));
        }

        /// <summary>
        /// Performs a text search in a specific entity type
        /// </summary>
        /// <param name="q">The search query to perform</param>
        /// <param name="typeName">The Entity Type to search. If omitted, search on all types</param>
        /// <param name="fields">The list of fields to return. Only valid when specifying a TypeName. When searching in all types the fields returned are fixed</param>
        /// <param name="paging">Paging setting for the query</param>
        /// <returns></returns>
        public async Task<Data.Search> Search(string q, string typeName, string[] fields, Paging paging)
        {
            return await Search(new Data.Request.Search(q, typeName, fields, paging));
        }

        /// <summary>
        /// Creates a discussion message, link it related entities and notify users or groups
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <param name="relatedEntities">Entity Ids</param>
        /// <param name="notify">Entity Ids for users or groups to notify</param>
        /// <param name="topics">Entity Ids</param>
        /// <returns></returns>
        public async Task<Data.CreateDiscussion> CreateDiscussion(object entity, string[] relatedEntities, string[] notify, string[] topics)
        {
            return await CreateDiscussion(new Data.Request.CreateDiscussion(entity, relatedEntities, notify, topics));
        }

        /// <summary>
        /// Creates a discussion message, link it related entities and notify users or groups
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <param name="relatedEntities">Entity Ids</param>
        /// <param name="notify">Entity Ids for users or groups to notify</param>
        /// <returns></returns>
        public async Task<Data.CreateDiscussion> CreateDiscussion(object entity, string[] relatedEntities, string[] notify)
        {
            return await CreateDiscussion(new Data.Request.CreateDiscussion(entity, relatedEntities, notify));
        }

        /// <summary>
        /// Creates a discussion message and link it related entities
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <param name="relatedEntities">Entity Ids</param>
        /// <returns></returns>
        public async Task<Data.CreateDiscussion> CreateDiscussion(object entity, string[] relatedEntities)
        {
            return await CreateDiscussion(new Data.Request.CreateDiscussion(entity, relatedEntities));
        }

        /// <summary>
        /// Creates a discussion message
        /// </summary>
        /// <param name="entity">Discussion message to be created</param>
        /// <returns></returns>
        public async Task<Data.CreateDiscussion> CreateDiscussion(object entity)
        {
            return await CreateDiscussion(new Data.Request.CreateDiscussion(entity));
        }

        /// <summary>
        /// Creates a discussion message, link it related entities and notify users or groups
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.CreateDiscussion> CreateDiscussion(Data.Request.CreateDiscussion request)
        {
            Data.CreateDiscussion apiCall = new Data.CreateDiscussion(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "CreateDiscussion", "createDiscussion call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Performs life cycle operations (Activate, Cancel etc.) on an entity
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.Lifecycle> Lifecycle(Data.Request.Lifecycle request)
        {
            Data.Lifecycle apiCall = new Data.Lifecycle(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "Lifecycle", "lifecycle call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Performs life cycle operations on an entity. For timesheets, possible operations are Submit, Approve and Reopen. For other entities these could be Activate, Cancel etc.
        /// </summary>
        /// <param name="ids">A list of objects (Entity Ids) to perform the operation on</param>
        /// <param name="operation">The operation to perform ('Activate', 'Cancel' etc.)</param>
        /// <returns></returns>
        public async Task<Data.Lifecycle> Lifecycle(string[] ids, string operation)
        {
            return await Lifecycle(new Data.Request.Lifecycle(ids, operation));
        }

        /// <summary>
        /// Changes the state of an object
        /// </summary>
        /// <param name="ids">List of objects to perform the operation on</param>
        /// <param name="state">The new state</param>
        /// <returns></returns>
        public async Task<Data.ChangeState> ChangeState(string[] ids, string state)
        {
            Data.ChangeState apiCall = new Data.ChangeState(new Data.Request.ChangeState(ids, state), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "ChangeState", "changeState call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Executes custom action
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.ExecuteCustomAction> ExecuteCustomAction(Data.Request.ExecuteCustomAction request)
        {
            Data.ExecuteCustomAction apiCall = new Data.ExecuteCustomAction(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "ExecuteCustomAction", "executeCustomAction call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Executes custom action
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="customAction"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<Data.ExecuteCustomAction> ExecuteCustomAction(string targetId, string customAction, FieldValue[] values)
        {
            return await ExecuteCustomAction(new Data.Request.ExecuteCustomAction(targetId, customAction, values));
        }

        /// <summary>
        /// Returns the list of template available for a certain Entity Type
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public async Task<Data.GetTemplateDescriptions> GetTemplateDescriptions(string typeName)
        {
            Data.GetTemplateDescriptions apiCall = new Data.GetTemplateDescriptions(new Data.Request.GetTemplateDescriptions(typeName), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetTemplateDescriptions", "getTemplateDescriptions call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Creates an entity from a predefined template
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Data.CreateFromTemplate> CreateFromTemplate(object entity, string templateName, string parentId)
        {
            Data.CreateFromTemplate apiCall = new Data.CreateFromTemplate(new Data.Request.CreateFromTemplate(entity, templateName, parentId), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "CreateFromTemplate", "createFromTemplate call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Provides information about calendar definitions in Clarizen. This API can provide information about the organization calendar or a user calendar
        /// </summary>
        /// <param name="id">The id of the entity (organization or user) to get the calendar info for</param>
        /// <returns></returns>
        public async Task<Data.GetCalendarInfo> GetCalendarInfo(string id)
        {
            Data.GetCalendarInfo apiCall = new Data.GetCalendarInfo(new Data.Request.GetCalendarInfo(id), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetCalendarInfo", "getCalendarInfo call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        public async Task<Data.GetCalendarExceptions> GetCalendarExceptions(string id, DateTime fromDate, DateTime toDate)
        {
            Data.GetCalendarExceptions apiCall = new Data.GetCalendarExceptions(new Data.Request.GetCalendarExceptions(id, fromDate, toDate), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetCalendarExceptions", "GetCalendarExceptions call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Returns the missing timesheets of a user between given dates
        /// </summary>
        /// <param name="user">EntityId of the User. If querying a multi-instance environment use /Organization/orgId/User/userId</param>
        /// <param name="startDate">The start of the date range (inclusive)</param>
        /// <param name="endDate">The end of the date range (exclusive)</param>
        /// <returns></returns>
        public async Task<Data.GetMissingTimesheets> GetMissingTimesheets(string user, DateTime startDate, DateTime endDate)
        {
            Data.GetMissingTimesheets apiCall = new Data.GetMissingTimesheets(new Data.Request.GetMissingTimesheets(user, startDate, endDate), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetMissingTimesheets", "GetMissingTimesheets call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        #endregion Data - Other methods

        #region CZQL

        /// <summary>
        /// Executes a Clarizen Query Language (CZQL) query. Visit https://api.clarizen.com/V2.0/services/data/Query for more information.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<Data.Query> ExecuteQuery(Data.Request.Query query)
        {
            Data.Query apiCall = new Data.Query(query, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "ExecuteQuery", "query call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Executes a Clarizen Query Language (CZQL) query. Visit https://api.clarizen.com/V2.0/services/data/Query for more information.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<Data.Query> ExecuteQuery(Interfaces.IClarizenQuery query)
        {
            return await ExecuteQuery(new Data.Request.Query(query.ToCZQL()));
        }

        #endregion CZQL

        #region Bulk Service

        /// <summary>
        /// Starts the bulk service. API calls are not executed until CommitBulkService function is called.
        /// </summary>
        public void StartBulkService()
        {
            IsBulk = true;
            BulkRequests = new List<Request> { };
        }

        /// <summary>
        /// Executes all the calls in the buffer in a single Clarizen bulk API call.
        /// </summary>
        /// <param name="transactional"></param>
        /// <param name="batch"></param>
        /// <param name="includeRequestsInResponses">Embed requests in responses so that when there is an error in a bulk operation you can look into the request that caused it</param>
        /// <returns></returns>
        public async Task<Bulk.Execute> CommitBulkService(bool transactional = false, bool? batch = null, bool? includeRequestsInResponses = null, int? timeout = null)
        {
            Logs.Assert(IsBulk, "Ekin.Clarizen.API", "CommitBulkService", "Bulk service not started");
            if (IsBulk)
            {
                CallSettings callSettings = CallSettings.GetFromAPI(this);
                callSettings.Timeout = timeout;
                Bulk.Execute apiCall = new Bulk.Execute(new Bulk.Request.Execute(BulkRequests, transactional, batch), callSettings);
                apiCall.SessionTimeout += Call_SessionTimeout;
                bool executionResult = await apiCall.Execute();
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "CommitBulkService", "Bulk service error", apiCall.Error);
                TotalAPICallsMadeInCurrentSession++;
                if (executionResult)
                {
                    if (apiCall.Data.Responses?.Length > 0)
                    {
                        for (int n = 0; n < apiCall.Data.Responses.Length; n++)
                        {
                            if (apiCall.Data.Responses[n].StatusCode == 200)
                                apiCall.Data.Responses[n].CastBody(BulkRequests[n].ResultType);
                            else
                                apiCall.Data.Responses[n].CastBodyToError();

                            if (includeRequestsInResponses.GetValueOrDefault(false))
                            {
                                // For every request in the payload Clarizen returns a response so their indexes must match
                                apiCall.Data.Responses[n].Request = BulkRequests[n];
                            }
                        }
                    }
                    else
                    {
                        Logs.AddError("Ekin.Clarizen.API", "CommitBulkService", "Bulk service executed successfully but no response was received from Clarizen");
                    }
                }
                return apiCall;
            }
            return null;
        }

        /// <summary>
        /// Deletes the current Bulk buffer
        /// </summary>
        public void CancelBulkService()
        {
            IsBulk = false;
            BulkRequests = new List<Request> { };
        }

        #endregion Bulk Service

        #region Utils

        /// <summary>
        /// Converts an API session to a web application session. Calling this method returns a url to the application that will not require credentials to login
        /// </summary>
        /// <returns></returns>
        public async Task<Utils.AppLogin> AppLogin()
        {
            Utils.AppLogin apiCall = new Utils.AppLogin(CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "AppLogin", "appLogin call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
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
        public async Task<Utils.SendEmail> SendEmail(Recipient[] recipients, string subject, string body, string relatedEntityId, Utils.Request.SendEmail.CZAccessType accessType)
        {
            Utils.SendEmail apiCall = new Utils.SendEmail(new Utils.Request.SendEmail(recipients, subject, body, relatedEntityId, accessType), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "SendEmail", "SendEmail call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        #endregion Utils

        #region Applications

        public async Task<Applications.GetApplicationStatus> GetApplicationStatus(string applicationId)
        {
            Applications.GetApplicationStatus apiCall = new Applications.GetApplicationStatus(new Applications.Request.GetApplicationStatus(applicationId), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetApplicationStatus", "getApplicationStatus call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        public async Task<Applications.InstallApplication> InstallApplication(string applicationId, bool autoEnable)
        {
            Applications.InstallApplication apiCall = new Applications.InstallApplication(new Applications.Request.InstallApplication(applicationId, autoEnable), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "InstallApplication", "installApplication call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        #endregion Applications

        #region Files

        /// <summary>
        /// Gets download information about a file attached to a document
        /// </summary>
        /// <param name="documentId">Entity Id</param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        public async Task<Files.Download> Download(string documentId, bool redirect)
        {
            Files.Download apiCall = new Files.Download(new Files.Request.Download(documentId, redirect), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "Download", "download call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Get a URL for uploading files. After POSTing a file to this URL, perform an Upload operation and pass this URL in the UploadUrl parameter.
        /// </summary>
        /// <returns></returns>
        public async Task<Files.GetUploadUrl> GetUploadUrl()
        {
            Files.GetUploadUrl apiCall = new Files.GetUploadUrl(CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "GetUploadUrl", "getUploadUrl call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Upload file to a document in Clarizen
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Files.Upload> Upload(Files.Request.Upload request)
        {
            Files.Upload apiCall = new Files.Upload(request, CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "Upload", "upload call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        /// <summary>
        /// Upload file to a document in Clarizen
        /// </summary>
        /// <param name="documentId">Id of a document to attach to</param>
        /// <param name="fileInformation">Additional information about the file</param>
        /// <param name="uploadUrl">When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl</param>
        /// <returns></returns>
        public async Task<Files.Upload> Upload(string documentId, FileInformation fileInformation, string uploadUrl)
        {
            return await Upload(new Files.Request.Upload(documentId, fileInformation, uploadUrl));
        }

        /// <summary>
        /// Upload file to a document in Clarizen
        /// </summary>
        /// <returns></returns>
        public async Task<Files.Upload> Upload(string documentId, StorageType storage, string url, string fileName, string subType, string extendedInfo, string uploadUrl)
        {
            return await Upload(new Files.Request.Upload(documentId, new FileInformation(storage, url, fileName, subType, extendedInfo), uploadUrl));
        }

        /// <summary>
        /// Set (Or Reset) the image of an object in Clarizen
        /// </summary>
        /// <param name="entityId">Id of an entity to attach to</param>
        /// <param name="uploadUrl">When the file is stored in Clarizen, provide the Url you received in a previous request to getUploadUrl</param>
        /// <param name="reset">Revert image to default icon</param>
        /// <returns></returns>
        public async Task<Files.UpdateImage> UpdateImage(string entityId, string uploadUrl, bool reset)
        {
            Files.UpdateImage apiCall = new Files.UpdateImage(new Files.Request.UpdateImage(entityId, uploadUrl, reset), CallSettings.GetFromAPI(this));
            apiCall.SessionTimeout += Call_SessionTimeout;
            bool executionResult = await apiCall.Execute();
            if (IsBulk)
            {
                BulkRequests.Add(apiCall.BulkRequest);
            }
            else 
            { 
                Logs.Assert(executionResult, "Ekin.Clarizen.API", "UpdateImage", "updateImage call failed", apiCall.Error); 
                TotalAPICallsMadeInCurrentSession++; 
            }
            return apiCall;
        }

        #endregion Files
    }

    public class SessionRefreshedEventArgs : EventArgs
    {
        public string NewSessionId { get; set; }
    }
}