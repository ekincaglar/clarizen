using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Authentication
{
    public class getServerDefinition
    {
        public Result.getServerDefinition Data { get; set; }
        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }

        public getServerDefinition(Request.getServerDefinition request, bool isSandbox)
        {
            Call(request, isSandbox);
        }
        public async Task Call(Request.getServerDefinition request, bool isSandbox)
        {
            Ekin.Rest.Client restClient = new Ekin.Rest.Client(isSandbox ?
                "https://apie.clarizentb.com/V2.0/services/authentication/getServerDefinition" :
                "https://api.clarizen.com/V2.0/services/authentication/getServerDefinition");
            restClient.ErrorType = typeof(error);
            Ekin.Rest.Response response = await restClient.Post(request, true);
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    this.Data = JsonConvert.DeserializeObject<Result.getServerDefinition>(response.Content);
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