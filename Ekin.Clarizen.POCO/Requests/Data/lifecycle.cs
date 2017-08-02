using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class lifecycle
    {
        /// <summary>
        /// A list of objects (Entity Ids) to perform the operation on
        /// </summary>
        public string[] ids { get; set; }
        /// <summary>
        /// The operation to perform ('Activate', 'Cancel' etc.)
        /// </summary>
        public string operation { get; set; }

        public lifecycle(string[] ids, string operation)
        {
            this.ids = ids;
            this.operation = operation;
        }
    }
}