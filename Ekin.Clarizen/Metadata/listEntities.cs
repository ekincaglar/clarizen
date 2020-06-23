namespace Ekin.Clarizen.Metadata
{
    public class ListEntities : Call<Result.ListEntities>
    {
        public ListEntities(CallSettings callSettings)
        {
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/listEntities";
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}