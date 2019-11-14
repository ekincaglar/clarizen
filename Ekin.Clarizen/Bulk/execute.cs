using System;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Bulk
{
    public class execute
    {
        public Result.execute Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }

        public execute(Request.execute request, CallSettings callSettings)
        {
            // Set the URL
            string url = callSettings.serverLocation + "/bulk/execute";

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", callSettings.sessionId));

            // Set the CallOptions header
            if (request.batch != null)
            {
                headers.Add("CallOptions", string.Format("Batch={0}", ((bool)request.batch) ? "true" : "false"));
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, callSettings.timeout.GetValueOrDefault(120000), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request, callSettings.serializeNullValues);

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.execute>(response.Content);
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
                this.Error = $"{response.InternalError.GetFormattedErrorMessage()}. Timeout set to {TimeSpan.FromMilliseconds(callSettings.timeout.GetValueOrDefault(120000)).ToHumanReadableString()}.";
            }
        }
    }
}