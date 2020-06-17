using System;

namespace Ekin.Clarizen.Data
{
    public class getCalendarExceptions : Call<Result.getCalendarExceptions>
    {
        public getCalendarExceptions(Request.getCalendarExceptions request, CallSettings callSettings)
        {
            if (request == null || request.fromDate == DateTime.MinValue || request.toDate == DateTime.MinValue)
            {
                IsCalledSuccessfully = false;
                this.Error = "FromDate and toDate must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = string.Format("{0}?{1}fromDate={2:yyyy-MM-dd}&toDate={3:yyyy-MM-dd}",
                   (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/getCalendarExceptions?",
                   string.IsNullOrWhiteSpace(request.entityId) ? "" : "entityId=" + request.entityId + "&",
                   request.fromDate,
                   request.toDate);
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}