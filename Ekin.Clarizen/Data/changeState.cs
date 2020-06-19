namespace Ekin.Clarizen.Data
{
    public class ChangeState : Call<dynamic>
    {
        public ChangeState(Request.ChangeState request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/changeState";
            _method = RequestMethod.Post;
        }
    }
}