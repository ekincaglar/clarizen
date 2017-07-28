using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Authentication.Result
{
    public class getServerDefinition
    {
        /// <summary>
        /// The actual API url to use for subsequent calls
        /// </summary>
        public string serverLocation { get; set; }

        public string appLocation { get; set; }

        public Int64 organizationId { get; set; }

        public getServerDefinition()
        {

        }
    }
}