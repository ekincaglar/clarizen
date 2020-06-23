namespace Ekin.Clarizen.Data
{
    public class CrossOrgEntityQuery : Call<Result.EntityQuery>
    {
        public CrossOrgEntityQuery(Queries.CrossOrgEntityQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/crossOrgEntityQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}