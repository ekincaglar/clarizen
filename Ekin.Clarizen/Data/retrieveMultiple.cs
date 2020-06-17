using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class retrieveMultiple : Call<Result.retrieveMultiple>
    {
        public retrieveMultiple(Request.retrieveMultiple request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/retrieveMultiple";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}