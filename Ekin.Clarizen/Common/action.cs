namespace Ekin.Clarizen
{
    public class Action
    {
        /// <summary>
        /// The url of the web service to call
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// The http method
        /// Possible values: GET | PUT | POST | DELETE
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// The multiline formula that represents a list of the http headers in the following format: 
        /// HeaderName1: HeaderValue1
        /// HeaderName2: HeaderValue2
        /// </summary>
        public string Headers { get; set; }
        /// <summary>
        /// The formula that represents a body of the http request
        /// </summary>
        public string Body { get; set; }

        public Action() { }

        public Action (string url, string method, string headers, string body)
        {
            this.Url = url;
            this.Method = method;
            this.Headers = headers;
            this.Body = body;
        }

    }
}