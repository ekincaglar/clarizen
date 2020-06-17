using System;
using System.Threading.Tasks;

namespace Ekin.Clarizen.Authentication
{
    public class logout : Call<dynamic>
    {
        public logout(string serverLocation, string sessionId)
        {
            _callSettings = new CallSettings
            {
                sessionId = sessionId
            };
            _url = serverLocation + "/authentication/logout";
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}