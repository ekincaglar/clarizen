namespace Ekin.Clarizen.Data
{
    public class GetTemplateDescriptions : Call<Result.GetTemplateDescriptions>
    {
        public GetTemplateDescriptions(Request.GetTemplateDescriptions request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TypeName))
            {
                IsCalledSuccessfully = false;
                this.Error = "Type name must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/getTemplateDescriptions?typeName=" + request.TypeName;
            _method = System.Net.Http.HttpMethod.Get;
        }
    }
}