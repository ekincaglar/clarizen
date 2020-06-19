namespace Ekin.Clarizen
{
    public class CallSettings
    {
        public string ServerLocation { get; set; }
        public string SessionId { get; set; }
        public string ApiKey { get; set; }
        public int? Timeout { get; set; } = null;
        public bool IsBulk { get; set; } = false;
        public bool SerializeNullValues { get; set; } = false;
        public int Retry { get; set; } = 1;
        public int SleepBetweenRetries { get; set; } = 0;
        public string Requester { get; set; }
        public string Redirect { get; set; }
        public int? ConnectionLimit { get; set; } = null;
        public System.Net.WebHeaderCollection Headers { get; set; } = null;

        public CallSettings()
        {
        }

        public static CallSettings GetFromAPI(API api, int? timeout = null)
        {
            if (api != null)
            {
                return new CallSettings()
                {
                    ServerLocation = api.ServerLocation,
                    SessionId = api.SessionId,
                    ApiKey = api.ApiKey,
                    IsBulk = api.IsBulk,
                    SerializeNullValues = api.SerializeNullValues,
                    Retry = api.Retry,
                    SleepBetweenRetries = api.SleepBetweenRetries,
                    Timeout = (timeout != null) ? timeout : api.Timeout,
                    Requester = api.Requester,
                    Redirect = api.Redirect,
                    ConnectionLimit = api.ConnectionLimit
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
            if (!string.IsNullOrWhiteSpace(ApiKey))
            {
                headers.Add(System.Net.HttpRequestHeader.Authorization, string.Format("ApiKey {0}", ApiKey));
            }
            else if (!string.IsNullOrWhiteSpace(SessionId))
            {
                headers.Add(System.Net.HttpRequestHeader.Authorization, string.Format("Session {0}", SessionId));
            }
            if (!string.IsNullOrWhiteSpace(Requester))
            {
                headers.Add("ClzApiRequester", Requester);
            }
            if (!string.IsNullOrWhiteSpace(Redirect))
            {
                headers.Add("x-redirect", Redirect);
            }
            if (Headers != null)
            {
                headers.Add(Headers);
            }
            return headers;
        }
    }
}