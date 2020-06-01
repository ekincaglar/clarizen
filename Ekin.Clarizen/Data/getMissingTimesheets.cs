using System;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class getMissingTimesheets : ISupportBulk
    {
        public Result.getMissingTimesheets Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }

        public getMissingTimesheets(Request.getMissingTimesheets request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.user) || request.startDate == null || request.endDate == null)
            {
                IsCalledSuccessfully = false;
                this.Error = "user, startDate and endDate parameters must be provided";
                return;
            }

            // Set the URL
            string url = string.Format("{0}?{1}startDate={2:yyyy-MM-dd}&endDate={3:yyyy-MM-dd}{4}",
                (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/missingTimesheets",
                string.IsNullOrWhiteSpace(request.user) ? "" : "user=" + request.user + "&",
                request.startDate,
                request.endDate,
                (request.tolerance == null) ? "" : "?tolerance=" + request.tolerance.ToString());

            if (callSettings.isBulk)
            {
                this.BulkRequest = new request(url, requestMethod.Get, typeof(Result.getMissingTimesheets));
                return;
            }

            // Call the API
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(url, callSettings.GetHeaders(), callSettings.timeout.GetValueOrDefault(), callSettings.retry, callSettings.sleepBetweenRetries);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();

            // Parse Data
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.getMissingTimesheets>(response.Content);
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