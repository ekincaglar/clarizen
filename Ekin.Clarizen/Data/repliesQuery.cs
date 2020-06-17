namespace Ekin.Clarizen.Data
{
    public class repliesQuery : Call<Result.repliesQuery>
    {
        public repliesQuery(Queries.repliesQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/repliesQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}