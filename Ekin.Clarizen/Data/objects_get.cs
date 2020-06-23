using System;
using System.Net.Http;

namespace Ekin.Clarizen.Data
{
    public class Objects_get : Call<dynamic>
    {
        public Objects_get(Request.Objects_get request, CallSettings callSettings, bool returnRawResponse = false)
        {
            if (request == null || String.IsNullOrEmpty(request.Id))
            {
                IsCalledSuccessfully = false;
                this.Error = "Object id must be provided in the request";
                return;
            }

            _request = request;
            _callSettings = callSettings;
            _returnRawResponse = returnRawResponse;
            _url =  (callSettings.IsBulk ? string.Empty : callSettings.ServerLocation) + "/data/objects" +
                    (request.Id.Substring(0, 1) != "/" ? "/" : "") + request.Id +
                    //(request.fields != null ? "?" + request.fields.ToQueryString() : string.Empty);
                    (request.Fields != null ? "?fields=" + GetFieldList(request.Fields) : string.Empty);
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