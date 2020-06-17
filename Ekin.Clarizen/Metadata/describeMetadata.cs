using Ekin.Rest;

namespace Ekin.Clarizen.Metadata
{
    public class describeMetadata : Call<Result.describeMetadata>
    {
        public describeMetadata(Request.describeMetadata request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/describeMetadata" + (request != null ? "?" + request.ToQueryString() : string.Empty);
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}