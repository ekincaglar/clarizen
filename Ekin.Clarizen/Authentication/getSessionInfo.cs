using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Authentication
{
    public class getSessionInfo : Call<Result.getSessionInfo>
    {
        public getSessionInfo(string serverLocation, string sessionId)
        {
            _callSettings = new CallSettings
            {
                sessionId = sessionId,
                isBulk = false, // Force this call to be made as a single call
                serializeNullValues = true
            };
            _url = serverLocation + "/authentication/getSessionInfo";
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}