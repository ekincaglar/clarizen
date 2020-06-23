namespace Ekin.Clarizen.Data
{
    public class CreateFromTemplate : Call<Result.CreateFromTemplate>
    {
        public CreateFromTemplate(Request.CreateFromTemplate request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/createFromTemplate";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}