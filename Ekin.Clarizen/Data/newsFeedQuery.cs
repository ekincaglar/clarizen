namespace Ekin.Clarizen.Data
{
    public class NewsFeedQuery : Call<Result.NewsFeedQuery>
    {
        public NewsFeedQuery(Queries.NewsFeedQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/newsFeedQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}