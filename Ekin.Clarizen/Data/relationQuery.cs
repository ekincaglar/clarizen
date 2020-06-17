using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class relationQuery : Call<Result.relationQuery>
    {
        public relationQuery(Queries.relationQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/relationQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}