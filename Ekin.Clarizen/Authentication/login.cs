using System;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Authentication
{
    public class login
    {
        public Result.login Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }

        public login(string serverLocation, Request.login request)
        {
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(serverLocation + "/authentication/login");
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = restClient.Post(request, true);
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.login>(response.Content);
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
                this.Error = response.GetFormattedErrorMessage();
            }
        }
    }
}