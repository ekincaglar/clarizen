namespace Ekin.Clarizen.Metadata
{
    public class describeEntityRelations : Call<Result.describeEntityRelations>
    {
        public describeEntityRelations(Request.describeEntityRelations request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/describeEntityRelations";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}