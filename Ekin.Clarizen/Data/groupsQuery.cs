namespace Ekin.Clarizen.Data
{
    public class GroupsQuery : Call<Result.GroupsQuery>
    {
        public GroupsQuery(Queries.GroupsQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/groupsQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}