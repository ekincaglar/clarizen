using System;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ekin.Clarizen.Data
{
    public class entityQuery : ISupportBulk
    {
        public Result.entityQuery Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public entityQuery(Queries.entityQuery request, CallSettings callSettings)
        {
            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/entityQuery";

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Post, request, typeof(Result.entityQuery));
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", callSettings.sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request, callSettings.serializeNullValues);

            // Return result
            if (response == null)
            {
                this.IsCalledSuccessfully = false;
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.entityQuery>(response.Content, new JsonSerializerSettings()
                    {
                        Error = HandleDeserializationError
                    });

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
                this.Error = response.InternalError.GetFormattedErrorMessage();
            }
            else
            {
                this.IsCalledSuccessfully = false;
            }
        }

        public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
    }
}