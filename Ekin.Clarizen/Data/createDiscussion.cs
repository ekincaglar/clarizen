namespace Ekin.Clarizen.Data
{
    public class createDiscussion : Call<Result.createDiscussion>
    {
        public createDiscussion(Request.createDiscussion request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/createDiscussion";
            _method = requestMethod.Post;

            var result = Execute();
        }
     }
}