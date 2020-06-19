using System;
using Ekin.Rest;

namespace Ekin.Clarizen.Files
{
    public class Download : Call<Result.Download>
    {
        public Download(Request.Download request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.DocumentId))
            {
                IsCalledSuccessfully = false;
                this.Error = "Document id must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/files/download?documentId=" +
                    (request.DocumentId.Substring(0, 1) != "/" ? "/" : "") + request.DocumentId +
                    (request.Redirect ? "&" + request.Redirect.ToQueryString() : string.Empty);
            _method = RequestMethod.Get;
        }
    }
}