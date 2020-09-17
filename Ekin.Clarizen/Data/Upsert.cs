namespace Ekin.Clarizen.Data
{
    public class Upsert : Call<dynamic>
    {
        public Upsert(Request.Upsert request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/upsert";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}