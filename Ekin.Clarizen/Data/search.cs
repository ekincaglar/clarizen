using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ekin.Rest;

namespace Ekin.Clarizen.Data
{
    public class search : ISupportBulk
    {
        public Result.search Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public search(string serverLocation, string sessionId, Request.search request, bool isBulk = false)
        {
            if (request == null || String.IsNullOrWhiteSpace(request.q))
            {
                IsCalledSuccessfully = false;
                this.Error = "Search query must be provided";
                return;
            }

            // Set the URL
            string url = (isBulk ? String.Empty : serverLocation) + "/data/search?q=" + request.q +
                         (request.fields != null ? "&" + request.fields.ToQueryString() : String.Empty) +
                         (!String.IsNullOrWhiteSpace(request.typeName) ? "&" + request.typeName.ToQueryString() : String.Empty) +
                         (request.paging != null ? "&" + request.paging.ToQueryString() : String.Empty);

            if (isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get);
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.search>(response.Content);
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