using System;
using System.Threading.Tasks;
using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Data
{
    public class changeState : Call<dynamic>
    {
        public changeState(Request.changeState request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/changeState";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}