using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data
{
    public class getCalendarExceptions : ISupportBulk
    {
        public Result.getCalendarExceptions Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public getCalendarExceptions(Request.getCalendarExceptions request, CallSettings callSettings)
        {
            if (request == null || request.fromDate == DateTime.MinValue || request.toDate == DateTime.MinValue)
            {
                IsCalledSuccessfully = false;
                this.Error = "FromDate and toDate must be provided";
                return;
            }

            // Set the URL
            string url = string.Format("{0}?{1}fromDate={2:yyyy-MM-dd}&toDate={3:yyyy-MM-dd}",
                (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/getCalendarExceptions?",
                string.IsNullOrWhiteSpace(request.entityId) ? "" : "entityId=" + request.entityId + "&",
                request.fromDate,
                request.toDate);

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get, typeof(Result.getCalendarExceptions));
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
                    this.Data = JsonConvert.DeserializeObject<Result.getCalendarExceptions>(response.Content);
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