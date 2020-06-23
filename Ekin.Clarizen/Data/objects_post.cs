using System.Net.Http;

namespace Ekin.Clarizen.Data
{
    public class Objects_post : Call<dynamic>
    {
        public Objects_post(string id, object obj, CallSettings callSettings)
        {
            _request = obj;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/objects" +
                   (id.Length > 0 && id.Substring(0, 1) != "/" ? "/" : "") + id;
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}