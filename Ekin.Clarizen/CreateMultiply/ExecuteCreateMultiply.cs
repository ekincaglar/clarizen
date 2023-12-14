using System;

namespace Ekin.Clarizen.CreateMultiply
{
    public class ExecuteCreateMultiply : Call<Result.ExecuteCreateMultiply>
    {
        public ExecuteCreateMultiply(Request.ExecuteCreateMultiply request, CallSettings callSettings)
        {
            _request = request.RequestBody;

            _callSettings = callSettings;
            _callSettings.IsMultiply = request.IsMultiply;
            _callSettings.CallOptions = request.CallOptions;
            if (_callSettings.Timeout == null)
            {
                callSettings.Timeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
            }
            _url = callSettings.ServerLocation + "/data/createMultiply";
            _method = System.Net.Http.HttpMethod.Put;
        }
    }
}
