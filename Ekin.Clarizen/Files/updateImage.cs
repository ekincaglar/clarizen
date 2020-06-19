namespace Ekin.Clarizen.Files
{
    public class UpdateImage : Call<Result.UpdateImage>
    {
        public UpdateImage(Request.UpdateImage request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/files/updateImage";
            _method = RequestMethod.Post;
        }
    }
}