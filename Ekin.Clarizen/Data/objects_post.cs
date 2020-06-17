using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;

namespace Ekin.Clarizen.Data
{
    public class objects_post : Call<dynamic>
    {
        public objects_post(string id, object obj, CallSettings callSettings)
        {
            _request = obj;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/objects" +
                   (id.Length > 0 && id.Substring(0, 1) != "/" ? "/" : "") + id;
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}