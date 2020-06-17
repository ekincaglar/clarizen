using Ekin.Rest;

namespace Ekin.Clarizen.Metadata
{
    public class getSystemSettingsValues : Call<Result.describeMetadata>
    {
        public getSystemSettingsValues(Request.getSystemSettingsValues request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/metadata/getSystemSettingsValues?" + request.ToQueryString();
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}