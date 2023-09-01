namespace Ekin.Clarizen.Authentication
{
    public class GetSessionInfo : Call<Result.GetSessionInfo>
    {
        public GetSessionInfo(string serverLocation, string sessionId, string requester)
        {
            _callSettings = new CallSettings
            {
                SessionId = sessionId,
                IsBulk = false, // Force this call to be made as a single call
                SerializeNullValues = true,
                Requester = requester
            };
            _url = serverLocation + "/authentication/getSessionInfo";
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}