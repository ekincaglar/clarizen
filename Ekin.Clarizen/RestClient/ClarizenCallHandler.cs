using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ekin.Clarizen.RestClient
{
    public class ClarizenCallHandler : DelegatingHandler
    {
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(120);

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            int retry = request.GetRetry().GetValueOrDefault();
            TimeSpan? sleepBetweenRetries = request.GetSleepBetweenRetries();
            TimeSpan timeout = request.GetTimeout() ?? DefaultTimeout;
            Stopwatch stopWatch = new Stopwatch();
            HttpResponseMessage response = null;
            for (int i = 0; i < retry; i++)
            {
                using (var cts = GetCancellationTokenSource(timeout, cancellationToken))
                {
                    try
                    {
                        if (stopWatch.IsRunning) stopWatch.Reset();
                        stopWatch.Start();
                        response = await base.SendAsync(request, cts?.Token ?? cancellationToken).ConfigureAwait(false);
                        if (response.IsSuccessStatusCode)
                        {
                            return response;
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO: Handle 429 Too Many Requests and throttle by introducing delays

                        if (i == retry - 1)
                        {
                            stopWatch.Stop();
                            string requestDuration = retry > 1 ? $"in {stopWatch.Elapsed.ToHumanReadableString()} after {retry} retries of {timeout.ToHumanReadableString()} each, with {sleepBetweenRetries.GetValueOrDefault().ToHumanReadableString()} between retries" : $"in {stopWatch.Elapsed.ToHumanReadableString()} (Timeout set to {timeout.ToHumanReadableString()})";

                            if ((ex is OperationCanceledException || ex is TaskCanceledException) && !cancellationToken.IsCancellationRequested)
                            {
                                string sourceOfCancellation = ex is OperationCanceledException ? "Clarizen" : "the server (task cancelled)";
                                throw new TimeoutException($"Timeout error received from {sourceOfCancellation}: {request.RequestUri} did not respond {requestDuration}.");
                            }
                            else
                            {
                                throw new HttpRequestException($"{request.RequestUri} threw the following exception {requestDuration}.", ex);
                            }
                        }
                    }
                    finally
                    {
                        stopWatch.Stop();
                    }
                }
                if (sleepBetweenRetries != null)
                {
                    await Task.Delay(sleepBetweenRetries.GetValueOrDefault());
                }
            }
            return response;
        }

        private CancellationTokenSource GetCancellationTokenSource(TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                return null;  // No need to create a CTS if there's no timeout
            }
            else
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(timeout);
                return cts;
            }
        }
    }
}
