using System;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class executeCustomAction : ISupportBulk
    {
        public Result.executeCustomAction Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public executeCustomAction(Request.executeCustomAction request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/executeCustomAction";

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Post, request, typeof(Result.executeCustomAction));
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
                    this.Data = JsonConvert.DeserializeObject<Result.executeCustomAction>(response.Content);
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