using Newtonsoft.Json;
using System;

namespace Ekin.Clarizen
{
    public class Request
    {
        /// <summary>
        /// Url of an API call
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The HTTP method to use for this request (GET,POST,PUT or DELETE)
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The request body
        /// </summary>
        public object Body { get; set; }

        [JsonIgnore]
        public Type ResultType { get; set; }

        public Request(string url, RequestMethod method, object body, Type resultType)
        {
            SetParams(url, method, body, resultType);
        }

        public Request(string url, RequestMethod method, Type resultType)
        {
            SetParams(url, method, null, resultType);
        }

        public Request(string url, RequestMethod method)
        {
            SetParams(url, method, null, null);
        }

        private void SetParams(string url, RequestMethod method, object body, Type resultType)
        {
            Url = url;
            Method = method.ToEnumString();
            Body = body;
            ResultType = resultType;
        }

    }
}