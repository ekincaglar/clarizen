namespace Ekin.Clarizen.Data
{
    public class GetMissingTimesheets : Call<Result.GetMissingTimesheets>
    {
        public GetMissingTimesheets(Request.GetMissingTimesheets request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.User) || request.StartDate == null || request.EndDate == null)
            {
                IsCalledSuccessfully = false;
                this.Error = "user, startDate and endDate parameters must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = string.Format("{0}?{1}startDate={2:yyyy-MM-dd}&endDate={3:yyyy-MM-dd}{4}",
                (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/missingTimesheets",
                string.IsNullOrWhiteSpace(request.User) ? "" : "user=" + request.User + "&",
                request.StartDate,
                request.EndDate,
                (request.Tolerance == null) ? "" : "?tolerance=" + request.Tolerance.ToString());
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}