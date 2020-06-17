using System;

namespace Ekin.Clarizen.Metadata
{
    public class objects_delete : Call<dynamic>
    {
        public objects_delete(Request.objects_delete request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Entity id must be provided in the request";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/objects" +
                   (request.id.Substring(0, 1) != "/" ? "/" : "") + request.id;
            _method = requestMethod.Delete;

            var result = Execute();
        }
    }
}