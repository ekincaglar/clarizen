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
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(100);

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            int retry = request.GetRetry().GetValueOrDefault();
            TimeSpan? sleepBetweenRetries = request.GetSleepBetweenRetries();
            using (var cts = GetCancellationTokenSource(request, cancellationToken))
            {
                HttpResponseMessage response = null;
                for (int i = 0; i < retry; i++)
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
                            throw new TimeoutException();
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
                return response;
            }
        }

        private CancellationTokenSource GetCancellationTokenSource(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var timeout = request.GetTimeout() ?? DefaultTimeout;
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
