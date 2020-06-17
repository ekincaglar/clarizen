using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class newsFeedQuery : Call<Result.newsFeedQuery>
    {
        public newsFeedQuery(Queries.newsFeedQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/newsFeedQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}