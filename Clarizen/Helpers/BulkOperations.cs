using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ekin.Clarizen.Data.Queries.Conditions;
using Ekin.Log;

namespace Ekin.Clarizen
{
    /// <summary>
    /// This class is used for running multiple API calls in bulk.
    /// Example Usage:
    ///     API ClarizenAPI = new API();
    ///     if (!ClarizenAPI.Login("username", "password")) return;
    ///     ClarizenAPI.Bulk.Start();
    ///     bool result = true;
    ///     foreach (POCO.Timesheet timesheet in Timesheets)
    ///     {
    ///         ClarizenAPI.CreateObject(timesheet.id, timesheet);
    ///         result = result & ClarizenAPI.Bulk.CheckCommit();
    ///     }
    ///     result = result & ClarizenAPI.Bulk.Close();
    /// </summary>
    public class BulkOperations
    {
        public API ClarizenAPI { get; private set; }
        public LogFactory Logs { get; set; }

        internal int APIBulkCallCount = 0;
        internal bool _isBulkTransactional = false;
        internal bool _batch = false;
        internal bool _includeRequestsInResponse = false;
        internal int _timeout = 120000;

        public BulkOperations(API ClarizenAPI)
        {
            Logs = new LogFactory();
            this.ClarizenAPI = ClarizenAPI;
        }

        public void UseSession(API ClarizenAPI)
        {
            this.ClarizenAPI = ClarizenAPI;
        }

        #region Internal functions for sending the bulk commits to the API

        internal bool Commit(bool enableTimeAudit = false, int sleepTime = 0)
        {
            bool result = true;
            DateTime startTime = DateTime.Now;
            Ekin.Clarizen.Bulk.execute bulkService = ClarizenAPI.CommitBulkService(_isBulkTransactional, _batch, _includeRequestsInResponse, _timeout);
            if (bulkService.IsCalledSuccessfully)
            {
                foreach (response res in bulkService.Data.responses)
                {
                    if (res.statusCode != 200)
                    {
                        Logs.AddError("Ekin.Clarizen.BulkOperations", "CommitBulkService", "Bulk item failed. Error: " + ((error)res.body).formatted, _includeRequestsInResponse ? res.request : null);
                        result = false;
                    }
                }
            }
            else
            {
                Logs.AddError("Ekin.Clarizen.BulkOperations", "CommitBulkService", "Bulk service failed. Error: " + bulkService.Error);
                result = false;
            }

            DateTime endTime = DateTime.Now;
            if (enableTimeAudit)
            {
                Logs.AddAudit("Ekin.Clarizen.BulkOperations", "CommitBulkService", string.Format("Bulk API call completed in {0:0.00}s", (endTime - startTime).TotalSeconds));
            }
            if (sleepTime > 0)
            {
                Thread.Sleep(sleepTime * 1000);
            }
            return result;
        }

        internal List<T> Commit<T>(out bool hasErrors, bool enableTimeAudit = false, int sleepTime = 0)
        {
            List<T> result = new List<T> { };
            DateTime startTime = DateTime.Now;
            hasErrors = false;

            Ekin.Clarizen.Bulk.execute bulkService = ClarizenAPI.CommitBulkService(_isBulkTransactional, _batch, _includeRequestsInResponse, _timeout);
            if (bulkService.IsCalledSuccessfully)
            {
                foreach (response res in bulkService.Data.responses)
                {
                    if (res.statusCode != 200)
                    {
                        Logs.AddError("Ekin.Clarizen.BulkOperations", "CommitBulkServiceAndGetData", "Bulk item failed. Error: " + ((error)res.body).formatted, _includeRequestsInResponse ? res.request : null);
                        hasErrors = true;
                    }
                    else
                    {
                        try
                        {
                            if (res.body is T)
                            {
                                result.Add((T)res.body);
                            }
                            else
                            {
                                result.Add(default(T));
                            }
                        }
                        catch (Exception ex)
                        {
                            Logs.AddError("Ekin.Clarizen.BulkOperations", "CommitBulkServiceAndGetData", string.Format("Item returned from Clarizen API could not be parsed to type {0}. Error: {1}", typeof(T), ex.Message));
                            hasErrors = true;
                        }
                    }
                }
            }
            else
            {
                Logs.AddError("Ekin.Clarizen.BulkOperations", "CommitBulkServiceAndGetData", "Bulk service failed. Error: " + bulkService.Error);
                hasErrors = true;
            }

            DateTime endTime = DateTime.Now;
            if (enableTimeAudit)
            {
                Logs.AddAudit("Ekin.Clarizen.BulkOperations", "CommitBulkServiceAndGetData", string.Format("Bulk API call completed in {0:0.00}s", (endTime - startTime).TotalSeconds));
            }

            if (sleepTime > 0)
            {
                Thread.Sleep(sleepTime * 1000);
            }

            if (hasErrors)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        #endregion Internal functions for sending the bulk commits to the API

        #region Bulk Operations (Reset, CheckCommit, ForceCommit, Close)

        /// <summary>
        /// Starts the bulk service. After this point every operation on API is put in a bulk buffer and not sent to Clarizen until CheckCommit or Close functions are executed.
        /// </summary>
        /// <param name="isTransactional"></param>
        /// <param name="batch"></param>
        public void Start(bool isTransactional = false, bool batch = false, bool includeRequestsInResponse = false, int timeout = 120000)
        {
            _isBulkTransactional = isTransactional;
            _batch = batch;
            _includeRequestsInResponse = includeRequestsInResponse;
            _timeout = timeout;
            APIBulkCallCount = 0;
            ClarizenAPI.StartBulkService();
        }

        /// <summary>
        /// Checks if the items in the buffer has reached bulkSize and if it has those items are sent to the API.
        /// </summary>
        /// <param name="enableTimeAudit"></param>
        /// <param name="sleepTime">Seconds to wait between each API call</param>
        /// <param name="bulkSize">Number of items to send in a bulk call</param>
        /// <returns></returns>
        public bool CheckCommit(bool enableTimeAudit = false, int sleepTime = 0, int bulkSize = 100)
        {
            bool result = true;
            APIBulkCallCount++;
            if (APIBulkCallCount >= bulkSize)
            {
                result = Commit(enableTimeAudit, sleepTime);
                Start(_isBulkTransactional, _batch, _includeRequestsInResponse, _timeout);
            }
            return result;
        }

        /// <summary>
        /// Checks if the items in the bulk buffer has reached bulkSize and if it has those items are sent to the API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hasErrors">Identifies if the API returned any errors</param>
        /// <param name="enableTimeAudit">Log how long each operation takes</param>
        /// <param name="sleepTime">Seconds to wait between each API call</param>
        /// <param name="bulkSize">Number of items to send in a bulk call</param>
        /// <returns></returns>
        public List<T> CheckCommit<T>(out bool hasErrors, bool enableTimeAudit = false, int sleepTime = 0, int bulkSize = 100)
        {
            List<T> result = null;
            hasErrors = false;
            APIBulkCallCount++;
            if (APIBulkCallCount >= bulkSize)
            {
                result = Commit<T>(out hasErrors, enableTimeAudit, sleepTime);
                Start(_isBulkTransactional, _batch, _includeRequestsInResponse, _timeout);
            }
            return result;
        }

        /// <summary>
        /// Commits whatever is in the buffer to Clarizen. Ensure that you have less than 1000 items to commit.
        /// </summary>
        /// <param name="enableTimeAudit"></param>
        /// <returns></returns>
        public bool ForceCommit(bool enableTimeAudit = false)
        {
            bool result = Commit(enableTimeAudit);
            Start(_isBulkTransactional, _batch, _includeRequestsInResponse, _timeout);
            return result;
        }

        /// <summary>
        /// Commits whatever is in the buffer to Clarizen. Ensure that you have less than 1000 items to commit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hasErrors"></param>
        /// <param name="enableTimeAudit"></param>
        /// <returns></returns>
        public List<T> ForceCommit<T>(out bool hasErrors, bool enableTimeAudit = false)
        {
            hasErrors = false;
            List<T> result = Commit<T>(out hasErrors, enableTimeAudit);
            Start(_isBulkTransactional, _batch, _includeRequestsInResponse, _timeout);
            return result;
        }

        /// <summary>
        /// Sends any items in the bulk buffer to the API and then closes the bulk service.
        /// </summary>
        /// <param name="enableTimeAudit">Log how long each operation takes</param>
        /// <param name="sleepTime">Seconds to wait between each API call</param>
        /// <returns></returns>
        public bool Close(bool enableTimeAudit = false, int sleepTime = 0)
        {
            bool result = true;
            if (APIBulkCallCount > 0)
            {
                result = Commit(enableTimeAudit, sleepTime);
            }
            ClarizenAPI.CancelBulkService();
            return result;
        }

        /// <summary>
        /// Sends any items in the bulk buffer to the API and then closes the bulk service. Results are returned as objects of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hasErrors">Identifies if the API returned any errors</param>
        /// <param name="enableTimeAudit">Log how long each operation takes</param>
        /// <param name="sleepTime">Seconds to wait between each API call</param>
        /// <returns></returns>
        public List<T> Close<T>(out bool hasErrors, bool enableTimeAudit = false, int sleepTime = 0)
        {
            List<T> result = null;
            hasErrors = false;
            if (APIBulkCallCount > 0)
            {
                result = Commit<T>(out hasErrors, enableTimeAudit, sleepTime);
            }
            ClarizenAPI.CancelBulkService();
            return result;
        }

        #endregion Bulk Operations (Reset, CheckCommit, ForceCommit, Close)

        #region GetAll by type

        /// <summary>
        /// Get multiple objects of type T by providing an EntityName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityName">Name of the Clarizen entity, e.g. Timesheet, WorkItem, etc.</param>
        /// <param name="customCondition"></param>
        /// <returns></returns>
        public List<T> GetAll<T>(string entityName, string customCondition = "", int sleepTime = 0)
        {
            customCondition = customCondition?.Trim();
            cZQLCondition condition = null;
            if (!string.IsNullOrEmpty(customCondition))
            {
                condition = new cZQLCondition(customCondition);
            }
            GetAllResult result = ClarizenAPI.Clone().GetAll(entityName, typeof(T), condition, sleepTime);
            if (result == null || result.Errors.Any())
            {
                var detailedMsg = "Error: " + string.Join(System.Environment.NewLine, result.Errors.Select(i => i.message));
                Logs.AddError("Ekin.Clarizen.BulkOperations", "Get All " + entityName, detailedMsg);
                return null;
            }
            return (List<T>)result.Data;
        }

        /// <summary>
        /// Get multiple objects of type T by running a query (IClarizenQuery)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">Clarizen Query Language (CZQL) query</param>
        /// <param name="pageSize">Default size to be used for pagination</param>
        /// <returns></returns>
        public List<T> GetAll<T>(Ekin.Clarizen.Interfaces.IClarizenQuery query, int? pageSize = null, int? sleepTime = null)
        {
            GetAllResult result = ClarizenAPI.Clone().GetAll(query, typeof(T), pageSize, sleepTime);
            if (result == null || result.Errors.Any())
            {
                var detailedMsg = "Error: " + string.Join(System.Environment.NewLine, result.Errors.Select(i => i.message));
                Logs.AddError("Ekin.Clarizen.BulkOperations", "Get All (Query)", detailedMsg);
                return null;
            }
            return (List<T>)result.Data;
        }

        #endregion GetAll by type

        #region Execute queries in bulk

        /// <summary>
        /// Run multiple queries in bulk. This method returns the results recursively so if any of the queries result in pagination more bulk queries are executed until all data is retrieved.
        /// </summary>
        /// <param name="Queries">List of Clarizen Query Language (CZQL) queries</param>
        /// <param name="bulkSize">Number of items to send in a bulk call</param>
        /// <returns></returns>
        public async Task<List<Ekin.Clarizen.Data.Result.query>> BulkQuery(List<Ekin.Clarizen.Data.Request.query> Queries, int bulkSize = 100)
        {
            if (Queries?.Count == 0) return null;
            int totalQueryCount = 0;
            List<int> itemsThatNeedPagination = new List<int> { };

            List<Ekin.Clarizen.Data.Result.query> results = new List<Ekin.Clarizen.Data.Result.query> { };
            bool bulkHasErrors = false;
            Start(false, false);
            for (int QueryCount = 0; QueryCount < Queries.Count; QueryCount++)
            {
                Ekin.Clarizen.Data.Request.query query = Queries[QueryCount];
                if (string.IsNullOrWhiteSpace(query.q)) continue;
                ClarizenAPI.ExecuteQuery(query);
                List<Ekin.Clarizen.Data.Result.query> bulkResult = CheckCommit<Ekin.Clarizen.Data.Result.query>(out bulkHasErrors, true, 0, bulkSize);
                if (!bulkHasErrors)
                {
                    if (bulkResult != null)
                    {
                        totalQueryCount += QueryCount + 1;
                        results.AddRange(bulkResult);
                        foreach (Ekin.Clarizen.Data.Result.query queryResult in bulkResult)
                        {
                            if (queryResult.paging.hasMore)
                            {
                                query.paging = queryResult.paging;
                                itemsThatNeedPagination.Add(QueryCount);
                            }
                            else
                            {
                                query.q = null;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            if (!bulkHasErrors)
            {
                List<Ekin.Clarizen.Data.Result.query> bulkResult = Close<Ekin.Clarizen.Data.Result.query>(out bulkHasErrors, true, 0);
                if (!bulkHasErrors)
                {
                    if (bulkResult != null)
                    {
                        results.AddRange(bulkResult);
                        for (int QueryCount = 0; QueryCount < bulkResult.Count; QueryCount++)
                        {
                            Ekin.Clarizen.Data.Result.query queryResult = bulkResult[QueryCount];
                            if (queryResult.paging.hasMore)
                            {
                                Queries[totalQueryCount + QueryCount].paging = queryResult.paging;
                                itemsThatNeedPagination.Add(totalQueryCount + QueryCount);
                            }
                            else
                            {
                                Queries[totalQueryCount + QueryCount].q = null;
                            }
                        }
                    }
                }
            }

            if (bulkHasErrors)
            {
                return null;
            }

            if (itemsThatNeedPagination.Count > 0)
            {
                List<Ekin.Clarizen.Data.Result.query> subResults = await BulkQuery(Queries, bulkSize);
                if (subResults == null)
                {
                    return null;
                }
                for (int QueryCount = 0; QueryCount < subResults.Count; QueryCount++)
                {
                    int resultIndex = itemsThatNeedPagination[QueryCount];
                    if (subResults[QueryCount].entities != null)
                    {
                        List<dynamic> mergeItems = new List<dynamic>(results[resultIndex].entities);
                        mergeItems.AddRange(subResults[QueryCount].entities);
                        results[resultIndex].entities = mergeItems.ToArray();
                    }
                }
            }

            return results;
        }

        public enum NumberOfResultsExpectedPerResponse { NoItem, SingleItem, MultipleItems }

        /// <summary>
        /// Run multiple queries in bulk, parse the results to list of T and return the list. This method returns the results recursively so if any of the queries result in pagination more bulk queries are executed until all data is retrieved.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Queries">List of Clarizen Query Language (CZQL) queries</param>
        /// <param name="HowManyItems">Defines how many items to expect in each response</param>
        /// <param name="EntityNameForErrorMessages">Used in the error messages that are returned, e.g. No {EntityNameForErrorMessages} were found</param>
        /// <param name="CriteriaForErrorMessages">Used in the error messages that are returned. List count must match the Queries list count.</param>
        /// <param name="bulkSize">Number of items to send in a bulk call</param>
        /// <returns></returns>
        public async Task<List<T>> BulkQuery<T>(List<Ekin.Clarizen.Data.Request.query> Queries, NumberOfResultsExpectedPerResponse HowManyItems, string EntityNameForErrorMessages = null, List<string> CriteriaForErrorMessages = null, int bulkSize = 100)
        {
            List<Ekin.Clarizen.Data.Result.query> queryResult = await BulkQuery(Queries);

            if (string.IsNullOrWhiteSpace(EntityNameForErrorMessages))
                EntityNameForErrorMessages = typeof(T).Name;

            List<T> ParsedObjects = new List<T> { };
            if (queryResult?.Count == 0)
            {
                Logs.AddError("Ekin.Clarizen.BulkOperations", "BulkQuery<T>", EntityNameForErrorMessages + " could not be loaded. Check Clarizen Errors.");
                return null;
            }
            for (int itemCount = 0; itemCount < queryResult.Count; itemCount++)
            {
                if (queryResult[itemCount].entities?.Length == 0)
                {
                    if (HowManyItems != NumberOfResultsExpectedPerResponse.NoItem)
                    {
                        string criteria = CriteriaForErrorMessages.ElementAtOrDefault(itemCount) != null ? " (" + CriteriaForErrorMessages.ElementAtOrDefault(itemCount) + ")" : string.Empty;
                        Logs.AddWarning("Ekin.Clarizen.BulkOperations", "BulkQuery<T>", $"No {EntityNameForErrorMessages} were found in Clarizen with the given criteria{criteria}.");
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (queryResult[itemCount].entities.Length > 1 && HowManyItems == NumberOfResultsExpectedPerResponse.SingleItem)
                {
                    string criteria = CriteriaForErrorMessages.ElementAtOrDefault(itemCount) != null ? " (" + CriteriaForErrorMessages.ElementAtOrDefault(itemCount) + ")" : string.Empty;
                    Logs.AddWarning("Ekin.Clarizen.BulkOperations", "BulkQuery<T>", $"{queryResult[itemCount].entities?.Length} {EntityNameForErrorMessages} were found in Clarizen with the given criteria{criteria}. Those entities will be ignored as the system cannot distinguish between them.");
                }
                else
                {
                    foreach (dynamic entity in queryResult[itemCount].entities)
                    {
                        try
                        {
                            ParsedObjects.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(entity.ToString()));
                        }
                        catch (Exception ex)
                        {
                            Logs.AddError("Ekin.Clarizen.BulkOperations", "BulkQuery<T>", ex);
                            Logs.AddError("Ekin.Clarizen.BulkOperations", "BulkQuery<T>", $"The result returned from Clarizen could not be parsed as a {typeof(T).Name} object. See previous errors.");
                            return null;
                        }
                    }
                }
            }
            return ParsedObjects;
        }

        #endregion Execute queries in bulk
    }
}