using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Bulk
{
    public class execute
    {
        public Result.execute Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }

        public execute(string serverLocation, string sessionId, Request.execute request, int? timeout = null)
        {
            // Set the URL
            string url = serverLocation + "/bulk/execute";

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", sessionId));

            // Set the CallOptions header
            if (request.batch != null)
            {
                headers.Add("CallOptions", string.Format("Batch={0}", ((bool)request.batch) ? "true" : "false"));
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, timeout.GetValueOrDefault(120000));
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request);

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
                this.Error = response.InternalError.GetFormattedErrorMessage();
            }
        }

    }
}