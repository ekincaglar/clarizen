using Ekin.Clarizen.Data.Request;

namespace Ekin.Clarizen.Data
{
    public class DeletePermissions : Call<DeletePermission>
    {
        public DeletePermissions(object request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/DeletePermissions";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}
