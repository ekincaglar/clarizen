namespace Ekin.Clarizen.Data
{
    public class groupsQuery : Call<Result.groupsQuery>
    {
        public groupsQuery(Queries.groupsQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/groupsQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}