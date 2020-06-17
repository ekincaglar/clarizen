namespace Ekin.Clarizen.Authentication
{
    public class setPassword : Call<dynamic>
    {
        public setPassword(Request.setPassword request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/authentication/setPassword";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}