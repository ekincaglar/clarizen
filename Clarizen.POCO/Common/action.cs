namespace Ekin.Clarizen
{
    public class action
    {
        /// <summary>
        /// The url of the web service to call
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// The http method
        /// Possible values: GET | PUT | POST | DELETE
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// The multiline formula that represents a list of the http headers in the following format:
        /// HeaderName1: HeaderValue1
        /// HeaderName2: HeaderValue2
        /// </summary>
        public string headers { get; set; }

        /// <summary>
        /// The formula that represents a body of the http request
        /// </summary>
        public string body { get; set; }

        public action()
        {
        }

        public action(string url, string method, string headers, string body)
        {
            this.url = url;
            this.method = method;
            this.headers = headers;
            this.body = body;
        }
    }
}