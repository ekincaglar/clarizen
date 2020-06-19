namespace Ekin.Clarizen.Files
{
    public class Upload : Call<dynamic>
    {
        public Upload(Request.Upload request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/files/upload";
            _method = RequestMethod.Post;
        }
    }
}