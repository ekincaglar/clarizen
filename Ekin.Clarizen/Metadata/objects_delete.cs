using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Metadata
{
    public class objects_delete : ISupportBulk
    {
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public objects_delete(Request.objects_delete request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Entity id must be provided";
                return;
            }

            // Set the URL
            string url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/objects" +
                         (request.id.Substring(0, 1) != "/" ? "/" : "") + request.id;

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Delete);
                return;
            }

            // Set the header for the authenticated user
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", callSettings.sessionId));

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, headers, callSettings.timeout.GetValueOrDefault());
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Delete();

            // Return result
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                this.IsCalledSuccessfully = true;
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

    }
}