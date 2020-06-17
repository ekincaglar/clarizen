using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Ekin.Rest;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Files
{
    public class download : Call<Result.download>
    {
        public download(Request.download request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.documentId))
            {
                IsCalledSuccessfully = false;
                this.Error = "Document id must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/files/download?documentId=" +
                    (request.documentId.Substring(0, 1) != "/" ? "/" : "") + request.documentId +
                    (request.redirect ? "&" + request.redirect.ToQueryString() : string.Empty);
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}