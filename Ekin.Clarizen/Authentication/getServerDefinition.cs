namespace Ekin.Clarizen.Authentication
{
    public class getServerDefinition : Call<Result.getServerDefinition>
    {
        public getServerDefinition(Request.getServerDefinition request, bool isSandbox)
        {
            _request = request;
            _callSettings = new CallSettings
            {
                isBulk = false, // Force this call to be made as a single call
                serializeNullValues = true
            };
            _url = isSandbox ?
                "https://apie.clarizentb.com/V2.0/services/authentication/getServerDefinition" :
                "https://api.clarizen.com/V2.0/services/authentication/getServerDefinition";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}