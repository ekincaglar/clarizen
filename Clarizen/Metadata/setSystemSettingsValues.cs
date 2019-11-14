﻿using System;
using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Metadata
{
    public class setSystemSettingsValues : ISupportBulk
    {
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public setSystemSettingsValues(Request.setSystemSettingsValues request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/setSystemSettingsValues";

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Post, request, null);
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
                this.IsCalledSuccessfully = true;
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