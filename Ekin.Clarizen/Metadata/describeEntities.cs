using System;
using Ekin.Clarizen.Interfaces;
using Ekin.Rest;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Metadata
{
    public class describeEntities : ISupportBulk
    {
        public Result.describeEntities Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public describeEntities(Request.describeEntities request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/describeEntities?" + request.ToQueryString();

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get, typeof(Result.describeEntities));
                return;
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, callSettings.GetHeaders(), callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.describeEntities>(response.Content);
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
                this.Error = response.GetFormattedErrorMessage();
            }
        }
    }
}