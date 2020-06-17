namespace Ekin.Clarizen.Metadata
{
    public class listEntities : Call<Result.listEntities>
    {
        public listEntities(CallSettings callSettings)
        {
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/listEntities";
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}