namespace Ekin.Clarizen.Applications
{
    public class InstallApplication : Call<dynamic>
    {
        public InstallApplication(Request.InstallApplication request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/applications/installApplication";
            _method = RequestMethod.Post;
        }
    }
}