using System;

namespace Ekin.Clarizen.Data
{
    public class objects_get : Call<dynamic>
    {
        public objects_get(Request.objects_get request, CallSettings callSettings, bool returnRawResponse = false)
        {
            if (request == null || String.IsNullOrEmpty(request.id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Object id must be provided in the request";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _returnRawResponse = returnRawResponse;
            _url =  (callSettings.isBulk ? string.Empty : callSettings.serverLocation) + "/data/objects" +
                    (request.id.Substring(0, 1) != "/" ? "/" : "") + request.id +
                    //(request.fields != null ? "?" + request.fields.ToQueryString() : string.Empty);
                    (request.fields != null ? "?fields=" + GetFieldList(request.fields) : string.Empty);
            _method = requestMethod.Get;

            var result = Execute();
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