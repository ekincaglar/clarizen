namespace Ekin.Clarizen.Data
{
    public class Lifecycle : Call<dynamic>
    {
        public Lifecycle(Request.Lifecycle request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/lifecycle";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}