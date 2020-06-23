namespace Ekin.Clarizen.Data
{
    public class Query : Call<Result.Query>
    {
        public Query(Request.Query request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/query";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}