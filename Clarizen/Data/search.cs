﻿using System;
using Ekin.Clarizen.Interfaces;
using Ekin.Rest;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class search : ISupportBulk
    {
        public Result.search Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public search(Request.search request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.q))
            {
                IsCalledSuccessfully = false;
                this.Error = "Search query must be provided";
                return;
            }

            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/search?q=" + request.q +
                         (request.fields != null ? "&" + request.fields.ToQueryString() : string.Empty) +
                         (!string.IsNullOrWhiteSpace(request.typeName) ? "&" + request.typeName.ToQueryString() : string.Empty) +
                         (request.paging != null ? "&" + request.paging.ToQueryString() : string.Empty);

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get);
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", callSettings.sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.search>(response.Content);
                    this.IsCalledSuccessfully = true;
                }
                catch (Exception ex)
                {
                    this.IsCalledSuccessfully = false;
                    this.Error = ex.Message;
                }
            }
            else
            {
                this.IsCalledSuccessfully = false;
                this.Error = response.InternalError.GetFormattedErrorMessage();
            }
        }
    }
}