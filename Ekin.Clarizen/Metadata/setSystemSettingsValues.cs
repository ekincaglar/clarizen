namespace Ekin.Clarizen.Metadata
{
    public class setSystemSettingsValues : Call<dynamic>
    {
        public setSystemSettingsValues(Request.setSystemSettingsValues request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/setSystemSettingsValues";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}