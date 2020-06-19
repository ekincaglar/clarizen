namespace Ekin.Clarizen.Utils
{
    public class SendEmail : Call<dynamic>
    {
        public SendEmail(Request.SendEmail request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/utils/sendEMail";
            _method = RequestMethod.Post;
        }
    }
}