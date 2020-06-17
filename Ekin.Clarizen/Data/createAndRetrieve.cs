using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class createAndRetrieve : Call<Result.createAndRetrieve>
    {
        public createAndRetrieve(Request.createAndRetrieve request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/createAndRetrieve";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}