using System;
using System.Threading.Tasks;
using Ekin.Clarizen.Interfaces;

namespace Ekin.Clarizen.Applications
{
    public class installApplication : Call<dynamic>
    {
        public installApplication(Request.installApplication request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/applications/installApplication";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}