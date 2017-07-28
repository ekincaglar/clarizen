using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Authentication
{
    public class logout
    {
        public bool IsCalledSuccessfully { get; set; }

        public logout(string serverLocation, string sessionId) {
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", sessionId));
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(serverLocation + "/authentication/logout", headers);
            Ekin.Rest.Response response = restClient.Get();
            this.IsCalledSuccessfully = (response.Status == System.Net.HttpStatusCode.OK);
        }
    }
}