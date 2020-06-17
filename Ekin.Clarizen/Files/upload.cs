namespace Ekin.Clarizen.Files
{
    public class upload : Call<dynamic>
    {
        public upload(Request.upload request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/files/upload";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}