using System;

namespace Ekin.Clarizen.Data
{
    public class GetCalendarInfo : Call<Result.GetCalendarInfo>
    {
        public GetCalendarInfo(Request.GetCalendarInfo request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.Id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Entity id must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/getCalendarInfo?userId=" + request.Id;
            _method = RequestMethod.Get;
        }
    }
}