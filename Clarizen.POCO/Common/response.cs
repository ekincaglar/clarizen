using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ekin.Clarizen
{
    public class response
    {
        /// <summary>
        /// Status code (200 for OK, 500 for error)
        /// </summary>
        public int statusCode { get; set; }

        /// <summary>
        /// Response body as a JSON string
        /// </summary>
        public dynamic body { get; set; }

        /// <summary>
        /// There is an option to embed requests in responses so that when there is an error in a bulk operation you can look into the request that caused it
        /// </summary>
        public request request { get; set; }

        #region Type Casting

        [JsonIgnore]
        public string BodyType { get; set; }

        public void CastBody(Type type)
        {
            try
            {
                body = (body as JObject).ToObject(type);
                BodyType = type.FullName;
            }
            catch
            {
                // body & BodyType are unchanged
            }
        }

        public void CastBodyToError()
        {
            CastBody(typeof(error));
        }

        private string SerializeBody()
        {
            return JsonConvert.SerializeObject(body).ToString();
        }

        #endregion Type Casting
    }
}