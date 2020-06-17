using System;
using System.Threading.Tasks;
using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Ekin.Clarizen
{
    public class Call<T> : ISupportBulk
    {
        #region Local/Internal Properties

        internal string _url { get; set; }
        internal requestMethod _method { get; set; }
        internal object _request { get; set; }
        internal CallSettings _callSettings { get; set; }
        internal bool _returnRawResponse { get; set; }

        #endregion
        
        #region Public Properties

        public bool IsCalledSuccessfully { get; set; }
        public string Error { get; set; }
        public request BulkRequest { get; set; }
        public T Data { get; set; }

        #endregion

        public Call()
        {

        }

        public async Task<bool> Execute()
        {
            #region If the call is bulk, add this to the Bulk Request and exit this function

            if (_callSettings != null && _callSettings.isBulk)
            {
                //switch (_method)
                //{
                //    case requestMethod.Get:
                //    case requestMethod.Delete:
                //        //BulkRequest = new request(_url, _method);
                //        //BulkRequest = new request(_url, _method, null, typeof(T));
                //        BulkRequest = new request(_url, _method, typeof(T));
                //        break;

                //    case requestMethod.Post:
                //        //this.BulkRequest = new request(_url, _method, _request, null);
                //        this.BulkRequest = new request(_url, _method, _request, typeof(T));
                //        break;

                //    case requestMethod.Put:
                //        BulkRequest = new request(_url, _method, _request, typeof(T));
                //        break;
                //}
                this.BulkRequest = new request(_url, _method, _request, typeof(T));
                return true;
            }

            #endregion

            #region Call the Clarizen API

            // Set the Client
            Ekin.Rest.Client restClient = null;
            if (_callSettings == null)
            {
                restClient = new Ekin.Rest.Client(_url);
            }
            else
            {
                restClient = new Ekin.Rest.Client(_url, _callSettings.GetHeaders(), _callSettings.timeout.GetValueOrDefault(120000), _callSettings.retry, _callSettings.sleepBetweenRetries);
            }
            restClient.ErrorType = typeof(error);

            // Make the call
            Ekin.Rest.Response response = null;
            switch (_method)
            {
                case requestMethod.Get:
                    response = await restClient.Get();
                    break;

                case requestMethod.Post:
                    response = await restClient.Post(_request, (_callSettings?.serializeNullValues).GetValueOrDefault(false));
                    break;

                case requestMethod.Put:
                    response = await restClient.Put(_request, (_callSettings?.serializeNullValues).GetValueOrDefault(false));
                    break;

                case requestMethod.Delete:
                    response = await restClient.Delete();
                    break;

            }

            #endregion

            #region Parse and return the result

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (_returnRawResponse)
                {
                    Data = response.Content as dynamic;
                }
                else
                {
                    try
                    {
                        //Data = JsonConvert.DeserializeObject<T>(response.Content);

                        Data = JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings()
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
            else if (response.InternalError != null)
            {
                IsCalledSuccessfully = false;
                if (_callSettings?.timeout != null)
                {
                    Error = $"{response.GetFormattedErrorMessage()}. Timeout set to {TimeSpan.FromMilliseconds(_callSettings.timeout.GetValueOrDefault(120000)).ToHumanReadableString()}.";
                }
                else
                {
                    Error = response.GetFormattedErrorMessage();
                }
            }
            else
            {
                IsCalledSuccessfully = false;
            }

            return IsCalledSuccessfully;

            #endregion
        }

        public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            Error = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
    }
}