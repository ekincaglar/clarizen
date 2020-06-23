namespace Ekin.Clarizen.Authentication
{
    public class GetServerDefinition : Call<Result.GetServerDefinition>
    {
        public GetServerDefinition(Request.GetServerDefinition request, bool isSandbox)
        {
            _request = request;
            _callSettings = new CallSettings
            {
                IsBulk = false, // Force this call to be made as a single call
                SerializeNullValues = true
            };
            _url = isSandbox ?
                "https://apie.clarizentb.com/V2.0/services/authentication/getServerDefinition" :
                "https://api.clarizen.com/V2.0/services/authentication/getServerDefinition";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}