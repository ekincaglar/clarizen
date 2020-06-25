using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
            HttpResponseMessage response = null;
            for (int i = 0; i < retry; i++)
            {
                using (var cts = GetCancellationTokenSource(timeout, cancellationToken))
                {
                    try
                    {
                        response = await base.SendAsync(request, cts?.Token ?? cancellationToken);
                        if (response.IsSuccessStatusCode)
                        {
                            return response;
                        }
                    }
                    catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                    {
                        if (i == retry - 1)
                        {
                            string retries = retry > 1 ? $" after {retry} retries" : string.Empty;
                            throw new TimeoutException($"Timeout error: {request.RequestUri} did not respond in {timeout.ToHumanReadableString()}{retries}.");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        if (i == retry - 1)
                        {
                            throw ex;
                        }
                    }
                    if (sleepBetweenRetries != null)
                    {
                        await Task.Delay(sleepBetweenRetries.GetValueOrDefault());
                    }
                }
            }
            return response;

        }

        private CancellationTokenSource GetCancellationTokenSource(TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                // No need to create a CTS if there's no timeout
                return null;
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
