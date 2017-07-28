using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ekin.Rest;

namespace Ekin.Clarizen.Metadata
{
    public class getSystemSettingsValues : ISupportBulk
    {
        public Result.getSystemSettingsValues Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public getSystemSettingsValues(string serverLocation, string sessionId, Request.getSystemSettingsValues request, bool isBulk = false)
        {
            // Set the URL
            string url = (isBulk ? String.Empty : serverLocation) + "/metadata/getSystemSettingsValues?" + request.ToQueryString();

            if (isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get, typeof(Result.describeMetadata));
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.getSystemSettingsValues>(response.Content);
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