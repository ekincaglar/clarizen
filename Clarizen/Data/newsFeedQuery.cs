﻿using System;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class newsFeedQuery : ISupportBulk
    {
        public Result.newsFeedQuery Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public newsFeedQuery(Queries.newsFeedQuery request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/newsFeedQuery";

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Post, request, typeof(Result.newsFeedQuery));
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", callSettings.sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request, callSettings.serializeNullValues);

            // Return result
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.newsFeedQuery>(response.Content);
                    this.IsCalledSuccessfully = true;
                }
                catch (Exception ex)
                {
                    this.IsCalledSuccessfully = false;
                    this.Error = ex.Message;
                }
            }
            else if (response.InternalError != null)
            {
                this.IsCalledSuccessfully = false;
                this.Error = response.InternalError.GetFormattedErrorMessage();
            }
            else
            {
                this.IsCalledSuccessfully = false;
            }
        }
    }
}