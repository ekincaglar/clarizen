using System;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Metadata
{
    public class describeEntityRelations : ISupportBulk
    {
        public Result.describeEntityRelations Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public describeEntityRelations(Request.describeEntityRelations request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/describeEntityRelations";

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Post, request, typeof(Result.describeEntityRelations));
                return;
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, callSettings.GetHeaders(), callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request, callSettings.serializeNullValues);

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.describeEntityRelations>(response.Content);
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