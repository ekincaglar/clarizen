using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Bulk
{
    public class execute : Call<Result.execute>
    {
        public execute(Request.execute request, CallSettings callSettings)
        {
            _request = request;

            _callSettings = callSettings;
            _callSettings.isBulk = false; // Force this call to be made as a single call
            if (request.batch != null)
            {
                _callSettings.Headers = new System.Net.WebHeaderCollection();
                _callSettings.Headers.Add("CallOptions", string.Format("Batch={0}", ((bool)request.batch) ? "true" : "false"));
            }

            _url = callSettings.serverLocation + "/bulk/execute";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}