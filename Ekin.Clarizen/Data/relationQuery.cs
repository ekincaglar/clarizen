namespace Ekin.Clarizen.Data
{
    public class RelationQuery : Call<Result.RelationQuery>
    {
        public RelationQuery(Queries.RelationQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/relationQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}