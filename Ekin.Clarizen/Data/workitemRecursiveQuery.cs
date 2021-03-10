using System;

namespace Ekin.Clarizen.Data
{
    public class WorkitemRecursiveQuery : Call<Result.WorkitemRecursiveQuery>
    {
        public WorkitemRecursiveQuery(Queries.WorkitemRecursiveQuery request, CallSettings callSettings)
        {
            if (request == null || String.IsNullOrEmpty(request.TypeName))
            {
                IsCalledSuccessfully = false;
                this.Error = "Object typeName must be provided in the request";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            //_returnRawResponse = returnRawResponse;
            _url = (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/workItemRecursiveQuery" +
                    (request.TypeName != null ? "?typeName=" + request.TypeName : string.Empty) +
                    (request.RootItemId != null ? "&rootItemId=" + System.Web.HttpUtility.UrlEncode(request.RootItemId.Id) : string.Empty) +
                    (request.Fields != null ? "&fields=" + GetFieldList(request.Fields) : string.Empty);
            _method = System.Net.Http.HttpMethod.Get;
        }

        private string GetFieldList(string[] fields)
        {
            string ret = string.Empty;
            if (fields != null)
            {
                foreach (string field in fields)
                {
                    if (!string.IsNullOrWhiteSpace(ret))
                    {
                        ret += ",";
                    }
                    ret += field;
                }
            }
            return ret;
        }
    }
}
