namespace Ekin.Clarizen.Data
{
    public class aggregateQuery : Call<Result.aggregateQuery>
    {
        public aggregateQuery(Queries.aggregateQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/aggregateQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}