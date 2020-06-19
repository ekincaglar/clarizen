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
        internal RequestMethod _method { get; set; }
        internal object _request { get; set; }
        internal CallSettings _callSettings { get; set; }
        internal bool _returnRawResponse { get; set; }

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

        public Call()
        {

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

            #region Call the Clarizen API

            // Set the Client
            Ekin.Rest.Client restClient = null;
            if (_callSettings == null)
            {
                restClient = new Ekin.Rest.Client(_url);
            }
            else
            {
                restClient = new Ekin.Rest.Client(_url, _callSettings.GetHeaders(), _callSettings.Timeout.GetValueOrDefault(120000), _callSettings.Retry, _callSettings.SleepBetweenRetries);
            }
            restClient.ErrorType = typeof(Error);

            // Make the call
            Ekin.Rest.Response response = null;
            switch (_method)
            {
                case RequestMethod.Get:
                    response = await restClient.Get();
                    break;

                case RequestMethod.Post:
                    response = await restClient.Post(_request, (_callSettings?.SerializeNullValues).GetValueOrDefault(false));
                    break;

                case RequestMethod.Put:
                    response = await restClient.Put(_request, (_callSettings?.SerializeNullValues).GetValueOrDefault(false));
                    break;

                case RequestMethod.Delete:
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
                Error internalError = response.InternalError as Error;
                if (_method == RequestMethod.Get && internalError?.ErrorCode != null && internalError.ErrorCode.Equals("EntityNotFound"))
                {
                    // Clarizen returns Http 500 when an entity of the given type is not found
                    // In the body of the response "errorCode": "EntityNotFound" is returned
                    // This is where we handle that case
                    IsCalledSuccessfully = true;
                }
                else
                {
                    IsCalledSuccessfully = false;
                }

                if (_callSettings?.Timeout != null)
                {
                    Error = $"{response.GetFormattedErrorMessage()}. Timeout set to {TimeSpan.FromMilliseconds(_callSettings.Timeout.GetValueOrDefault(120000)).ToHumanReadableString()}.";
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