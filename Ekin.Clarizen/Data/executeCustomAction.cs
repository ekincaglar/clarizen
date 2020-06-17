using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class executeCustomAction : Call<Result.executeCustomAction>
    {
        public executeCustomAction(Request.executeCustomAction request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/executeCustomAction";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}