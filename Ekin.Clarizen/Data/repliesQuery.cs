namespace Ekin.Clarizen.Data
{
    public class RepliesQuery : Call<Result.RepliesQuery>
    {
        public RepliesQuery(Queries.RepliesQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/repliesQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}