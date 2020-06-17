namespace Ekin.Clarizen.Authentication
{
    public class login : Call<Result.login>
    {
        public login(string serverLocation, Request.login request)
        {
            _request = request;
            _callSettings = new CallSettings
            {
                isBulk = false, // Force this call to be made as a single call
                serializeNullValues = true
            };
            _url = serverLocation + "/authentication/login";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}