namespace Ekin.Clarizen.Utils
{
    public class AppLogin : Call<Result.AppLogin>
    {
        public AppLogin(CallSettings callSettings)
        {
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/utils/appLogin";
            _method = RequestMethod.Get;
        }
    }
}