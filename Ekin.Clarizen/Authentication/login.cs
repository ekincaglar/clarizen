namespace Ekin.Clarizen.Authentication
{
    public class Login : Call<Result.Login>
    {
        public Login(string serverLocation, Request.Login request)
        {
            _request = request;
            _callSettings = new CallSettings
            {
                IsBulk = false, // Force this call to be made as a single call
                SerializeNullValues = true
            };
            _url = serverLocation + "/authentication/login";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}