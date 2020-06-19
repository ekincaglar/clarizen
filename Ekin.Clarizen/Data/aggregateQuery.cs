namespace Ekin.Clarizen.Data
{
    public class AggregateQuery : Call<Result.AggregateQuery>
    {
        public AggregateQuery(Queries.AggregateQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/aggregateQuery";
            _method = RequestMethod.Post;
        }
    }
}