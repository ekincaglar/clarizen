using System;
using System.Threading.Tasks;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class objects_put : ISupportBulk
    {
        public Result.objects_put Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public objects_put(string id, object obj, CallSettings callSettings)
        {
            Call(id, obj, callSettings);
        }
        public async Task Call(string id, object obj, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/objects" +
                         (id.Substring(0, 1) != "/" ? "/" : "") + id;

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Put, obj, typeof(Result.objects_put));
                return;
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, callSettings.GetHeaders(), callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = await restClient.Put(obj, callSettings.serializeNullValues);

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.objects_put>(response.Content);
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