using Ekin.Rest;

namespace Ekin.Clarizen.Metadata
{
    public class GetSystemSettingsValues : Call<Result.DescribeMetadata>
    {
        public GetSystemSettingsValues(Request.GetSystemSettingsValues request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/metadata/getSystemSettingsValues?" + request.ToQueryString();
            _method = RequestMethod.Get;
        }
    }
}