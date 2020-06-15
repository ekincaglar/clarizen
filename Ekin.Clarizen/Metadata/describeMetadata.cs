using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Ekin.Rest;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Metadata
{
    public class describeMetadata : ISupportBulk
    {
        public Result.describeMetadata Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public describeMetadata(Request.describeMetadata request, CallSettings callSettings)
        {
            Call(request, callSettings);
        }
        public async Task Call(Request.describeMetadata request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/describeMetadata" + (request != null ? "?" + request.ToQueryString() : string.Empty);

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get, null, typeof(Result.describeMetadata));
                return;
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, callSettings.GetHeaders(), callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = await restClient.Get();

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
                this.Error = response.GetFormattedErrorMessage();
            }
        }
    }
}