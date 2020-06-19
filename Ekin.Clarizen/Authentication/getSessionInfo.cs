namespace Ekin.Clarizen.Authentication
{
    public class GetSessionInfo : Call<Result.GetSessionInfo>
    {
        public GetSessionInfo(string serverLocation, string sessionId)
        {
            _callSettings = new CallSettings
            {
                SessionId = sessionId,
                IsBulk = false, // Force this call to be made as a single call
                SerializeNullValues = true
            };
            _url = serverLocation + "/authentication/getSessionInfo";
            _method = RequestMethod.Get;
        }
    }
}