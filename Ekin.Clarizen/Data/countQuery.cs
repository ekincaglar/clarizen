using System.Net.Http;

namespace Ekin.Clarizen.Data
{
    public class CountQuery : Call<Result.CountQuery>
    {
        public CountQuery(Request.CountQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/countQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}