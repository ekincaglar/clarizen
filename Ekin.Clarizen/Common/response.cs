using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Ekin.Clarizen
{
    public class Response
    {
        /// <summary>
        /// Status code (200 for OK, 500 for error)
        /// </summary>
        public int StatusCode { get; set; }
        /// <summary>
        /// Response body as a JSON string
        /// </summary>
        public dynamic Body { get; set; }

        /// <summary>
        /// There is an option to embed requests in responses so that when there is an error in a bulk operation you can look into the request that caused it
        /// </summary>
        public Request Request { get; set; }

        #region Type Casting

        [JsonIgnore]
        public string BodyType { get; set; }

        public void CastBody(Type type)
        {
            try
            {
                if (type != null)
                {
                    Body = (Body as JObject).ToObject(type);
                    BodyType = type.FullName;
                }
            }
            catch
            {
                // body & BodyType are unchanged
            }
        }

        public void CastBodyToError()
        {
            CastBody(typeof(Error));
        }

        private string SerializeBody()
        {
            return JsonConvert.SerializeObject(Body);
        }

        #endregion

    }
}
