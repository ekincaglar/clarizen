namespace Ekin.Clarizen.Data
{
    public class entityQuery : Call<Result.entityQuery>
    {
        public entityQuery(Queries.entityQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/entityQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}