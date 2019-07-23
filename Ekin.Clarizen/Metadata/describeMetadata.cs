using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ekin.Rest;

namespace Ekin.Clarizen.Metadata
{
    public class describeMetadata : ISupportBulk
    {
        public Result.describeMetadata Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public describeMetadata(Request.describeMetadata request, CallSettings callSettings) {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/describeMetadata" + (request != null ? "?" + request.ToQueryString() : string.Empty);

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get, null, typeof(Result.describeMetadata));
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", callSettings.sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, callSettings.timeout.GetValueOrDefault());
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.describeMetadata>(response.Content);
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