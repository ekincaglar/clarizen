namespace Ekin.Clarizen.Utils
{
    public class sendEMail : Call<dynamic>
    {
        public sendEMail(Request.sendEMail request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/utils/sendEMail";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}