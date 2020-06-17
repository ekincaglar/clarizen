namespace Ekin.Clarizen.Authentication
{
    public class logout : Call<dynamic>
    {
        public logout(string serverLocation, string sessionId)
        {
            _callSettings = new CallSettings
            {
                sessionId = sessionId,
                isBulk = false  // Force this call to be made as a single call
            };
            _url = serverLocation + "/authentication/logout";
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}