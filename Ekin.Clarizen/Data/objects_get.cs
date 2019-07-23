using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ekin.Rest;

namespace Ekin.Clarizen.Data
{
    public class objects_get : ISupportBulk
    {
        public dynamic Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public objects_get(Request.objects_get request, CallSettings callSettings, bool returnRawResponse = false)
        {
            if (request == null || String.IsNullOrEmpty(request.id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Object id must be provided";
                return;
            }

            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/objects" +
                         (request.id.Substring(0, 1) != "/" ? "/" : "") + request.id +
                         //(request.fields != null ? "?" + request.fields.ToQueryString() : string.Empty);
                         (request.fields != null ? "?fields=" + GetFieldList(request.fields) : string.Empty);

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get);
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", callSettings.sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, callSettings.timeout.GetValueOrDefault());
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    if (returnRawResponse)
                    {
                        this.Data = response.Content;
                    }
                    else
                    {
                        this.Data = JsonConvert.DeserializeObject(response.Content);
                    }
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

        private string GetFieldList(string[] fields)
        {
            string ret = string.Empty;
            if (fields != null)
            {
                foreach (string field in fields)
                {
                    if (!string.IsNullOrWhiteSpace(ret))
                    {
                        ret += ",";
                    }
                    ret += field;
                }
            }
            return ret;
        }

    }
}