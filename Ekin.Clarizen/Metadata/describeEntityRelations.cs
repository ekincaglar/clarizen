namespace Ekin.Clarizen.Metadata
{
    public class DescribeEntityRelations : Call<Result.DescribeEntityRelations>
    {
        public DescribeEntityRelations(Request.DescribeEntityRelations request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/describeEntityRelations";
            _method = RequestMethod.Post;
        }
    }
}