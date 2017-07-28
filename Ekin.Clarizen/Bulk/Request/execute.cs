using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Bulk.Request
{
    public class execute
    {
        /// <summary>
        /// Array of Request objects representing individual API calls
        /// </summary>
        public List<request> requests { get; set; }
        public bool transactional { get; set; }

        public execute(List<request> requests, bool transactional)
        {
            this.requests = requests;
            this.transactional = transactional;
        }
    }
}