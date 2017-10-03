using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class error
    {
        /// <summary>
        /// An error code representing the failure that occured during the API call
        /// Possible values:
        /// EntityNotFound | InvalidArgument | MissingArgument | InvalidOperation | DuplicateKey | InvalidField | InvalidType | FileNotFound | VirusCheckFailed | Unauthorized | UnsupportedClient | General | Internal | ValidationRuleError | LoginFailure | SessionTimeout | Redirect | InvalidQuery
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// An error message that can be displayed on screen
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// A string that can be used to track this error on Clarizen's side. Make sure to include this when contacting support about issues with the API
        /// </summary>
        public string referenceId { get; set; }

        /// <summary>
        /// Returns a formatted string with all of the error information
        /// </summary>
        public string formatted
        {
            get
            {
                return String.Format("[{0}] {1} (Reference Id: {2})", errorCode, message, referenceId);
            }
        }

        public error() { }

        public error(string errorCode, string message)
        {
            this.errorCode = errorCode;
            this.message = message;
        }

    }
    
}