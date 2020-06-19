namespace Ekin.Clarizen.Data
{
    public class CreateAndRetrieve : Call<Result.CreateAndRetrieve>
    {
        public CreateAndRetrieve(Request.CreateAndRetrieve request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/createAndRetrieve";
            _method = RequestMethod.Post;
        }
    }
}