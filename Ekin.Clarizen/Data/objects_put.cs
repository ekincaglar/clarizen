using System;
using System.Threading.Tasks;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class objects_put : Call<Result.objects_put>
    {
        public objects_put(string id, object obj, CallSettings callSettings)
        {
            _request = obj;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/objects" +
                         (id.Substring(0, 1) != "/" ? "/" : "") + id;
            _method = requestMethod.Put;

            var result = Execute();
        }
    }
}