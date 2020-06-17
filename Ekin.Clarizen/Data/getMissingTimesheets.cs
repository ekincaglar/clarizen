namespace Ekin.Clarizen.Data
{
    public class getMissingTimesheets : Call<Result.getMissingTimesheets>
    {
        public getMissingTimesheets(Request.getMissingTimesheets request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.user) || request.startDate == null || request.endDate == null)
            {
                IsCalledSuccessfully = false;
                this.Error = "user, startDate and endDate parameters must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = string.Format("{0}?{1}startDate={2:yyyy-MM-dd}&endDate={3:yyyy-MM-dd}{4}",
                (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/missingTimesheets",
                string.IsNullOrWhiteSpace(request.user) ? "" : "user=" + request.user + "&",
                request.startDate,
                request.endDate,
                (request.tolerance == null) ? "" : "?tolerance=" + request.tolerance.ToString());
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}