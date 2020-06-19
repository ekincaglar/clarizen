using System;

namespace Ekin.Clarizen.Metadata
{
    public class Objects_delete : Call<dynamic>
    {
        public Objects_delete(Request.Objects_delete request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.Id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Entity id must be provided in the request";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/objects" +
                   (request.Id.Substring(0, 1) != "/" ? "/" : "") + request.Id;
            _method = RequestMethod.Delete;
        }
    }
}