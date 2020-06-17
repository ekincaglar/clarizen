namespace Ekin.Clarizen.Data
{
    public class entityFeedQuery : Call<Result.entityFeedQuery>
    {
        public entityFeedQuery(Queries.entityFeedQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/entityFeedQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}