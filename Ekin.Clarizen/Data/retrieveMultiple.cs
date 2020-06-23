namespace Ekin.Clarizen.Data
{
    public class RetrieveMultiple : Call<Result.RetrieveMultiple>
    {
        public RetrieveMultiple(Request.RetrieveMultiple request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/retrieveMultiple";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}