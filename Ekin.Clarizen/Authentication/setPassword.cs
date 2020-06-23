namespace Ekin.Clarizen.Authentication
{
    public class SetPassword : Call<dynamic>
    {
        public SetPassword(Request.SetPassword request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/authentication/setPassword";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}