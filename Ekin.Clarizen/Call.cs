using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ekin.Clarizen.Interfaces;
using Ekin.Clarizen.RestClient;
using Ekin.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ekin.Clarizen
{
    public class Call<T> : ISupportBulk, ICall<T>
    {
        #region Local/Internal Properties

        public HttpClient _httpClient { get; set; }
        public string _url { get; set; }
        public HttpMethod _method { get; set; }
        public object _request { get; set; }
        public CallSettings _callSettings { get; set; }
        public bool _returnRawResponse { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns true if the Http call to Clarizen was completed successfully, even if the result of the call has errors that Clarizen returned
        /// </summary>
        public bool IsCalledSuccessfully { get; set; } = false;

        /// <summary>
        /// Error message returned by Clarizen or the REST adapter
        /// </summary>
        public string Error { get; set; } = default;

        /// <summary>
        /// This is a queue item for bulk requests
        /// </summary>
        public Request BulkRequest { get; set; } = null;

        /// <summary>
        /// The data returned by the Clarizen API call
        /// </summary>
        public T Data { get; set; } = default;

        #endregion

        #region Events

        public event EventHandler SessionTimeout;

        public event EventHandler CallQuotaExceeded;

        #endregion

        public Call()
        {
            _httpClient = HttpClientExtensions.Client;
        }
        public Call(HttpClient Client)
        {
            _httpClient = Client;
        }

        /// <summary>
        /// Make a REST call to Clarizen's API
        /// </summary>
        /// <returns>Returns true if the Http call to Clarizen was completed successfully, even if the result of the call has errors that Clarizen returned</returns>
        public async Task<bool> Execute()
        {
            #region If the call is bulk, add this to the Bulk Request and exit this function

            if (_callSettings != null && _callSettings.IsBulk)
            {
                this.BulkRequest = new Request(_url, _method, _request, typeof(T));
                return true;
            }

            #endregion

            #region Create the Http request

            string contentBody = string.Empty;

            if (_method == HttpMethod.Post || _method == HttpMethod.Put)
            {
                try
                {
                    JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = (_callSettings?.AllowReferenceLoops).GetValueOrDefault(false) ? ReferenceLoopHandling.Serialize : ReferenceLoopHandling.Error,
                        NullValueHandling = (_callSettings?.SerializeNullValues).GetValueOrDefault(false) ? NullValueHandling.Include : NullValueHandling.Ignore
                    };

                    contentBody = JsonConvert.SerializeObject(_request, serializerSettings);
                }
                catch(Exception ex)
                {
                    IsCalledSuccessfully = false;
                    Error = "JSON Serialization Error: " + ex.ToString();
                }
            }

            var requestMessage = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new Uri(_url),
                Method = _method,
                Content = _method == HttpMethod.Post || _method == HttpMethod.Put ? new StringContent(contentBody, Encoding.UTF8, "application/json") : null
            };

            //requestMessage.Headers.Add("ContentType", "application/json; charset=utf-8");
            requestMessage.Headers.Add("Accept", "application/json");
            requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate");

            if (_callSettings != null)
            {
                #region Headers
                if (!string.IsNullOrWhiteSpace(_callSettings.ApiKey))
                {
                    requestMessage.Headers.Add("Authorization", string.Format("ApiKey {0}", _callSettings.ApiKey));
                }
                else if (!string.IsNullOrWhiteSpace(_callSettings.SessionId))
                {
                    requestMessage.Headers.Add("Authorization", string.Format("Session {0}", _callSettings.SessionId));
                }
                if (!string.IsNullOrWhiteSpace(_callSettings.Requester))
                {
                    requestMessage.Headers.Add("ClzApiRequester", _callSettings.Requester);
                }
                if (!string.IsNullOrWhiteSpace(_callSettings.Redirect))
                {
                    requestMessage.Headers.Add("x-redirect", _callSettings.Redirect);
                }
                if (_callSettings.IsBatch != null)
                {
                    requestMessage.Headers.Add("CallOptions", string.Format("Batch={0}", _callSettings.IsBatch.GetValueOrDefault() ? "true" : "false"));
                }
                #endregion

                #region Timeout
                if (_callSettings.Timeout != null)
                {
                    requestMessage.SetTimeout(TimeSpan.FromMilliseconds(_callSettings.Timeout.GetValueOrDefault()));
                }
                #endregion

                #region Retry
                requestMessage.SetRetry(_callSettings.Retry);

                if (_callSettings.SleepBetweenRetries > 0)
                {
                    requestMessage.SetSleepBetweenRetries(TimeSpan.FromMilliseconds(_callSettings.SleepBetweenRetries));
                }
                #endregion
            }

            #endregion

            #region Call the Clarizen API

            HttpResponseMessage response = null;
            string content = string.Empty;

            try
            {
                response = await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (TimeoutException ex)
            {
                Error = ex.Message; // Timeout exceptions are formatted inside ClarizenCallHandler
                IsCalledSuccessfully = false;
                return IsCalledSuccessfully;
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
                IsCalledSuccessfully = false;
                return IsCalledSuccessfully;
            }

            #endregion

            #region Parse and return the result

            if (response.IsSuccessStatusCode)
            {
                if (_returnRawResponse)
                {
                    Data = content as dynamic;
                    IsCalledSuccessfully = true;
                }
                else
                {
                    try
                    {
                        Data = JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings()
                        {
                            Error = HandleDeserializationError
                        });

                        IsCalledSuccessfully = true;
                    }
                    catch (Exception ex)
                    {
                        IsCalledSuccessfully = false;
                        Error = ex.Message;
                    }
                }
            }
            else
            {
                Ekin.Clarizen.Error clarizenError = null;
                try
                {
                    clarizenError = JsonConvert.DeserializeObject<Ekin.Clarizen.Error>(content);
                }
                catch
                {

                }
                if (clarizenError != null)
                {
                    if (clarizenError.Message.Contains("API calls quota exceeded", StringComparison.InvariantCultureIgnoreCase))
                    {
                        clarizenError.ErrorCode = "CallQuotaExceeded";
                    }
                    if (string.IsNullOrWhiteSpace(clarizenError.ReferenceId))
                    {
                        clarizenError.ReferenceId = "Not specified";
                    }
                    Error = clarizenError.Formatted;
                    if (clarizenError.ErrorCode != null)
                    {
                        switch (clarizenError.ErrorCode)
                        {
                            case "EntityNotFound":
                                if (_method == HttpMethod.Get)
                                {
                                    // Clarizen returns Http 500 when an entity of the given type is not found
                                    // In the body of the response "errorCode": "EntityNotFound" is returned
                                    // This is where we handle that case
                                    IsCalledSuccessfully = true;
                                }
                                break;

                            case "SessionTimeout":
                                OnSessionTimeout(EventArgs.Empty);
                                break;

                            case "CallQuotaExceeded":
                                OnCallQuotaExceeded(EventArgs.Empty);
                                break;
                        }
                    }
                }
                else
                {
                    Error = content;
                    IsCalledSuccessfully = false;
                }
            }

            return IsCalledSuccessfully;

            #endregion
        }

        protected virtual void OnSessionTimeout(EventArgs e)
        {
            SessionTimeout?.Invoke(this, e);
        }

        protected virtual void OnCallQuotaExceeded(EventArgs e)
        {
            CallQuotaExceeded?.Invoke(this, e);
        }

        /// <summary>
        /// Handles Newtonsoft.Json's deserialization error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="errorArgs"></param>
        public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            Error = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
    }
}