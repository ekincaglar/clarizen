namespace Ekin.Clarizen.Data
{
    public class createFromTemplate : Call<Result.createFromTemplate>
    {
        public createFromTemplate(Request.createFromTemplate request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/createFromTemplate";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}