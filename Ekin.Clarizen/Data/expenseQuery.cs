namespace Ekin.Clarizen.Data
{
    public class expenseQuery : Call<Result.expenseQuery>
    {
        public expenseQuery(Queries.expenseQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/expenseQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}