using System;

namespace Ekin.Clarizen.Data
{
    public class GetCalendarExceptions : Call<Result.GetCalendarExceptions>
    {
        public GetCalendarExceptions(Request.GetCalendarExceptions request, CallSettings callSettings)
        {
            if (request == null || request.FromDate == DateTime.MinValue || request.ToDate == DateTime.MinValue)
            {
                IsCalledSuccessfully = false;
                this.Error = "FromDate and toDate must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = string.Format("{0}?{1}fromDate={2:yyyy-MM-dd}&toDate={3:yyyy-MM-dd}",
                   (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/getCalendarExceptions?",
                   string.IsNullOrWhiteSpace(request.EntityId) ? "" : "entityId=" + request.EntityId + "&",
                   request.FromDate,
                   request.ToDate);
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}