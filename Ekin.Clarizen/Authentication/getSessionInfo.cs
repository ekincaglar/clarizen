using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Authentication
{
    public class getSessionInfo
    {
        public Result.getSessionInfo Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }

        public getSessionInfo(string serverLocation, string sessionId) {
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            headers.Add(System.Net.HttpRequestHeader.Authorization, String.Format("Session {0}", sessionId));
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(serverLocation + "/authentication/getSessionInfo", headers);
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Get();
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.getSessionInfo>(response.Content);
                    this.IsCalledSuccessfully = true;
                }
                catch (Exception ex)
                {
                    this.IsCalledSuccessfully = false;
                    this.Error = ex.Message;
                }
            }
            else
            {
                this.IsCalledSuccessfully = false;
                this.Error = response.InternalError.GetFormattedErrorMessage();
            }
        }
    }
}