using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ekin.Clarizen.RestClient
{
    public static class HttpClientExtensions
    {
        public static readonly HttpClient Client = new HttpClient(new ClarizenCallHandler { InnerHandler = new HttpClientHandler() });

        private const string TimeoutPropertyKey = "RequestTimeout";
        private const string RetryKey = "Retry";
        private const string SleepBetweenRetriesKey = "SleepBetweenRetries";
        private const int MaxRetries = 3;

        public static void SetTimeout(this HttpRequestMessage request, TimeSpan? timeout)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            request.Properties[TimeoutPropertyKey] = timeout;
        }

        public static TimeSpan? GetTimeout(this HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Properties.TryGetValue(TimeoutPropertyKey, out var value) && value is TimeSpan timeout)
                return timeout;
            return null;
        }

        public static void SetRetry(this HttpRequestMessage request, int? timeout)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            request.Properties[RetryKey] = timeout;
        }

        public static int? GetRetry(this HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Properties.TryGetValue(RetryKey, out var value) && value is int retry)
                return retry;
            return MaxRetries;
        }

        public static void SetSleepBetweenRetries(this HttpRequestMessage request, TimeSpan? sleepBetweenRetries)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            request.Properties[SleepBetweenRetriesKey] = sleepBetweenRetries;
        }

        public static TimeSpan? GetSleepBetweenRetries(this HttpRequestMessage request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Properties.TryGetValue(SleepBetweenRetriesKey, out var value) && value is TimeSpan sleepBetweenRetries)
                return sleepBetweenRetries;
            return null;
        }
    }
}
