using System;

namespace Ekin.Clarizen
{
    public class Error
    {
        /// <summary>
        /// An error code representing the failure that occured during the API call
        /// Possible values:
        /// EntityNotFound | InvalidArgument | MissingArgument | InvalidOperation | DuplicateKey | InvalidField | InvalidType | FileNotFound | VirusCheckFailed | Unauthorized | UnsupportedClient | General | Internal | ValidationRuleError | LoginFailure | SessionTimeout | Redirect | InvalidQuery
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// An error message that can be displayed on screen
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// A string that can be used to track this error on Clarizen's side. Make sure to include this when contacting support about issues with the API
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Returns a formatted string with all of the error information
        /// </summary>
        public string Formatted
        {
            get
            {
                return String.Format("[{0}] {1} (Reference Id: {2})", ErrorCode, Message, ReferenceId);
            }
        }

        public Error() { }

        public Error(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }

    }
    
}