namespace Ekin.Clarizen.Files
{
    public class updateImage : Call<Result.updateImage>
    {
        public updateImage(Request.updateImage request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/files/updateImage";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}