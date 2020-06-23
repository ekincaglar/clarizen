namespace Ekin.Clarizen.Metadata
{
    public class DescribeMetadata : Call<Result.DescribeMetadata>
    {
        public DescribeMetadata(Request.DescribeMetadata request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/describeMetadata" + (request != null ? "?" + request.ToQueryString() : string.Empty);
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}