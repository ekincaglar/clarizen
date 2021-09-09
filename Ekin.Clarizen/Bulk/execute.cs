using System;

namespace Ekin.Clarizen.Bulk
{
    public class Execute : Call<Result.Execute>
    {
        public Execute(Request.Execute request, CallSettings callSettings)
        {
            _request = request;

            _callSettings = callSettings;
            _callSettings.IsBulk = false; // Force this call to be made as a single call
            _callSettings.IsBatch = request.Batch.GetValueOrDefault();
            if (_callSettings.Timeout == null)
            {
                callSettings.Timeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
            }
            _url = callSettings.ServerLocation + "/bulk/execute";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}