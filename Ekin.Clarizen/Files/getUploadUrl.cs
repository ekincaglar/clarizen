namespace Ekin.Clarizen.Files
{
    public class GetUploadUrl : Call<Result.GetUploadUrl>
    {
        public GetUploadUrl(CallSettings callSettings)
        {
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/files/getUploadUrl";
            _method = RequestMethod.Get;
        }
    }
}