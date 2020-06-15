using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Utils
{
    public class appLogin : ISupportBulk
    {
        public Result.appLogin Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public appLogin(CallSettings callSettings)
        {
            Call(callSettings);
        }
        public async Task Call(CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/utils/appLogin";

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get, typeof(Result.appLogin));
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
                    this.Data = JsonConvert.DeserializeObject<Result.appLogin>(response.Content);
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