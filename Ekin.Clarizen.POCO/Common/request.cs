using Newtonsoft.Json;
using System;
using Ekin.Clarizen.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class request
    {
        /// <summary>
        /// Url of an API call
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// The HTTP method to use for this request (GET,POST,PUT or DELETE)
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// The request body
        /// </summary>
        public object body { get; set; }

        [JsonIgnore]
        public Type resultType { get; set; }

        public request(string url, requestMethod method, object body, Type resultType)
        {
            SetParams(url, method, body, resultType);
        }

        public request(string url, requestMethod method, Type resultType)
        {
            SetParams(url, method, null, resultType);
        }

        public request(string url, requestMethod method)
        {
            SetParams(url, method, null, null);
        }

        private void SetParams(string url, requestMethod method, object body, Type resultType)
        {
            this.url = url;
            this.method = method.ToEnumString();
            this.body = body;
            this.resultType = resultType;
        }

    }
}