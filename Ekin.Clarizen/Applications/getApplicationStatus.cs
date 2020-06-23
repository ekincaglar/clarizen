namespace Ekin.Clarizen.Applications
{
    public class GetApplicationStatus : Call<Result.GetApplicationStatus>
    {
        public GetApplicationStatus(Request.GetApplicationStatus request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/applications/getApplicationStatus";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}