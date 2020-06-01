using System;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Applications
{
    public class getApplicationStatus : ISupportBulk
    {
        public Result.getApplicationStatus Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public getApplicationStatus(Request.getApplicationStatus request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/applications/getApplicationStatus";

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Post, request, typeof(Result.getApplicationStatus));
                return;
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, callSettings.GetHeaders(), callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request, callSettings.serializeNullValues);

            // Return result
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.getApplicationStatus>(response.Content);
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
                this.Error = response.GetFormattedErrorMessage();
            }
            else
            {
                this.IsCalledSuccessfully = false;
            }
        }
    }
}