namespace Ekin.Clarizen.Applications
{
    public class getApplicationStatus : Call<Result.getApplicationStatus>
    {
        public getApplicationStatus(Request.getApplicationStatus request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/applications/getApplicationStatus";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}