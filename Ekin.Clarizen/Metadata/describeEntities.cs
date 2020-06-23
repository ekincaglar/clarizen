namespace Ekin.Clarizen.Metadata
{
    public class DescribeEntities : Call<Result.DescribeEntities>
    {
        public DescribeEntities(Request.DescribeEntities request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/describeEntities?" + request.ToQueryString();
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}