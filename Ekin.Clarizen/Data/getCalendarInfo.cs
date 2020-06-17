using System;

namespace Ekin.Clarizen.Data
{
    public class getCalendarInfo : Call<Result.getCalendarInfo>
    {
        public getCalendarInfo(Request.getCalendarInfo request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Entity id must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/getCalendarInfo?userId=" + request.id;
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}