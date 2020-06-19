namespace Ekin.Clarizen.Data
{
    public class EntityFeedQuery : Call<Result.EntityFeedQuery>
    {
        public EntityFeedQuery(Queries.EntityFeedQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/entityFeedQuery";
            _method = RequestMethod.Post;
        }
    }
}