using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;

namespace Ekin.Clarizen.Data
{
    public class lifecycle : Call<dynamic>
    {
        public lifecycle(Request.lifecycle request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/lifecycle";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}