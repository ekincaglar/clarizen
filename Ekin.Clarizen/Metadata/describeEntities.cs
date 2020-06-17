using Ekin.Rest;

namespace Ekin.Clarizen.Metadata
{
    public class describeEntities : Call<Result.describeEntities>
    {
        public describeEntities(Request.describeEntities request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/describeEntities?" + request.ToQueryString();
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}