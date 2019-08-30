using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Authentication
{
    public class setPassword
    {
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }

        public setPassword(string serverLocation, Request.setPassword request)
        {
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(serverLocation + "/authentication/setPassword");
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request, true);
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                this.IsCalledSuccessfully = true;
            }
            else
            {
                this.IsCalledSuccessfully = false;
                this.Error = response.InternalError.GetFormattedErrorMessage();
            }
        }
    }
}