using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data
{
    public class lifecycle : ISupportBulk
    {
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public lifecycle(string serverLocation, string sessionId, Request.lifecycle request, bool isBulk = false) {
            // Set the URL
            string url = (isBulk ? string.Empty : serverLocation) + "/data/lifecycle";

            if (isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Post, request, null);
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request);

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