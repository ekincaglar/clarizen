namespace Ekin.Clarizen
{
    public class CallSettings
    {
        public string ServerLocation { get; set; }
        public string SessionId { get; set; }
        public string ApiKey { get; set; }
        public int? Timeout { get; set; } = null;
        public bool IsBulk { get; set; } = false;
        public bool? IsBatch { get; set; } = null;
        public bool SerializeNullValues { get; set; } = false;
        public bool AllowReferenceLoops { get; set; } = false;
        public int Retry { get; set; } = 1;
        public int SleepBetweenRetries { get; set; } = 0;
        public string Requester { get; set; }
        public string Redirect { get; set; }
        public int? ConnectionLimit { get; set; } = null;
        public bool? DisableWorkflowRules { get; set; } = null;
        public bool? DisableValidationRules { get; set; } = null;

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
    }
}