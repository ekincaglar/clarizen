namespace Ekin.Clarizen.Data
{
    public class ExecuteCustomAction : Call<Result.ExecuteCustomAction>
    {
        public ExecuteCustomAction(Request.ExecuteCustomAction request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/executeCustomAction";
            _method = System.Net.Http.HttpMethod.Post;
        }
    }
}