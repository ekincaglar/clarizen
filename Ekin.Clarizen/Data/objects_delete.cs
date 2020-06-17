using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;

namespace Ekin.Clarizen.Data
{
    public class objects_delete : Call<dynamic>
    {
        public objects_delete(Request.objects_delete request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Object id must be provided in the request";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/objects" +
                   (request.id.Substring(0, 1) != "/" ? "/" : "") + request.id;
            _method = requestMethod.Delete;

            var result = Execute();
        }
    }
}