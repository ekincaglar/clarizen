namespace Ekin.Clarizen
{
    public class CallSettings
    {
        public string serverLocation { get; set; }
        public string sessionId { get; set; }
        public string apiKey { get; set; }
        public int? timeout { get; set; } = null;
        public bool isBulk { get; set; } = false;
        public bool serializeNullValues { get; set; } = false;
        public int retry { get; set; } = 1;
        public int sleepBetweenRetries { get; set; } = 0;
        public string requester { get; set; }
        public string redirect { get; set; }

        public CallSettings()
        {
        }

        public static CallSettings GetFromAPI(API api, int? timeout = null)
        {
            if (api != null)
            {
                return new CallSettings()
                {
                    serverLocation = api.serverLocation,
                    sessionId = api.sessionId,
                    apiKey = api.ApiKey,
                    isBulk = api.isBulk,
                    serializeNullValues = api.serializeNullValues,
                    retry = api.retry,
                    sleepBetweenRetries = api.sleepBetweenRetries,
                    timeout = (timeout != null) ? timeout : api.timeout,
                    requester = api.Requester,
                    redirect = api.Redirect
                };
            }
            else
            {
                return new CallSettings();
            }
        }

        /// <summary>
        /// Get the http header for the authenticated user
        /// </summary>
        /// <returns></returns>
        public System.Net.WebHeaderCollection GetHeaders()
        {
            System.Net.WebHeaderCollection headers = new System.Net.WebHeaderCollection();
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                headers.Add(System.Net.HttpRequestHeader.Authorization, string.Format("ApiKey {0}", apiKey));
            }
            else if (!string.IsNullOrWhiteSpace(sessionId))
            {
                headers.Add(System.Net.HttpRequestHeader.Authorization, string.Format("Session {0}", sessionId));
            }
            if (!string.IsNullOrWhiteSpace(requester))
            {
                headers.Add("ClzApiRequester", requester);
            }
            if (!string.IsNullOrWhiteSpace(redirect))
            {
                headers.Add("x-redirect", redirect);
            }
            return headers;
        }
    }
}