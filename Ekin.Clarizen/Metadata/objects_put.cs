namespace Ekin.Clarizen.Metadata
{
    public class objects_put : Call<Result.objects_put>
    {
        public objects_put(Request.objects_put request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/objects";
            _method = requestMethod.Put;

            var result = Execute();
        }
    }
}