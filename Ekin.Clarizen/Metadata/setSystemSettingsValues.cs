namespace Ekin.Clarizen.Metadata
{
    public class SetSystemSettingsValues : Call<dynamic>
    {
        public SetSystemSettingsValues(Request.SetSystemSettingsValues request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/setSystemSettingsValues";
            _method = RequestMethod.Post;
        }
    }
}