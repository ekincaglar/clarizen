namespace Ekin.Clarizen.Utils
{
    public class appLogin : Call<Result.appLogin>
    {
        public appLogin(CallSettings callSettings)
        {
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/utils/appLogin";
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}