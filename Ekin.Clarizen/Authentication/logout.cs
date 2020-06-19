namespace Ekin.Clarizen.Authentication
{
    public class Logout : Call<dynamic>
    {
        public Logout(string serverLocation, string sessionId)
        {
            _callSettings = new CallSettings
            {
                SessionId = sessionId,
                IsBulk = false  // Force this call to be made as a single call
            };
            _url = serverLocation + "/authentication/logout";
            _method = RequestMethod.Get;
        }
    }
}