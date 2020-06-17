namespace Ekin.Clarizen.Data
{
    public class query : Call<Result.query>
    {
        public query(Request.query request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/query";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}