namespace Ekin.Clarizen.Data
{
    public class EntityQuery : Call<Result.EntityQuery>
    {
        public EntityQuery(Queries.EntityQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/entityQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}