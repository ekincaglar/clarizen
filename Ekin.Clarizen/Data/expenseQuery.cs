using System.Net.Http;

namespace Ekin.Clarizen.Data
{
    public class ExpenseQuery : Call<Result.ExpenseQuery>
    {
        public ExpenseQuery(Queries.ExpenseQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/expenseQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}