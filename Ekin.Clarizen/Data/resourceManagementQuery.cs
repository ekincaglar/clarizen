namespace Ekin.Clarizen.Data
{
    public class ResourceManagementQuery : Call<Result.ResourceManagementQuery>
    {
        public ResourceManagementQuery(Queries.ResourceManagementQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/resourceManagementQuery";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}
