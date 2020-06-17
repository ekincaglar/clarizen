namespace Ekin.Clarizen.Data
{
    public class countQuery : Call<Result.countQuery>
    {
        public countQuery(Request.countQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/countQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}