using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class getTemplateDescriptions : Call<Result.getTemplateDescriptions>
    {
        public getTemplateDescriptions(Request.getTemplateDescriptions request, CallSettings callSettings)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.typeName))
            {
                IsCalledSuccessfully = false;
                this.Error = "Type name must be provided";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/getTemplateDescriptions?typeName=" + request.typeName;
            _method = requestMethod.Get;

            var result = Execute();
        }
    }
}