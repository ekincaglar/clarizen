namespace Ekin.Clarizen.Metadata
{
    public class Objects_put : Call<Result.Objects_put>
    {
        public Objects_put(Request.Objects_put request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/objects";
            _method = RequestMethod.Put;
        }
    }
}