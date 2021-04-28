using Ekin.Clarizen.Data.Request;

namespace Ekin.Clarizen.Data
{
    public class SetPermissions : Call<SetPermission>
    {
        public SetPermissions(object request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/SetPermissions";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}
