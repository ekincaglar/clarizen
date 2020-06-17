using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Ekin.Rest;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class search : Call<Result.search>
    {
        public search(Request.search request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.q))
            {
                IsCalledSuccessfully = false;
                this.Error = "Search query must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/search?q=" + request.q +
                    (request.fields != null ? "&" + request.fields.ToQueryString() : string.Empty) +
                    (!string.IsNullOrWhiteSpace(request.typeName) ? "&" + request.typeName.ToQueryString() : string.Empty) +
                    (request.paging != null ? "&" + request.paging.ToQueryString() : string.Empty);
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}