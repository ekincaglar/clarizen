namespace Ekin.Clarizen.Data
{
    public class crossOrgEntityQuery : Call<Result.entityQuery>
    {
        public crossOrgEntityQuery(Queries.crossOrgEntityQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/crossOrgEntityQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}