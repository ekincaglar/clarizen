using System;
using Ekin.Clarizen.Interfaces;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ekin.Clarizen.Data
{
    public class timesheetQuery : Call<Result.timesheetQuery>
    {
        public timesheetQuery(Queries.timesheetQuery request, CallSettings callSettings)
        {
            _request = request;
            _callSettings = callSettings;
            _url = (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/timesheetQuery";
            _method = requestMethod.Post;

            var result = Execute();
        }
    }
}