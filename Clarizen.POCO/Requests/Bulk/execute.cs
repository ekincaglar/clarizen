using System.Collections.Generic;

namespace Ekin.Clarizen.Bulk.Request
{
    public class execute
    {
        /// <summary>
        /// Array of Request objects representing individual API calls
        /// </summary>
        public List<request> requests { get; set; }

        public bool transactional { get; set; }
        public bool? batch { get; set; }

        public execute(List<request> requests, bool transactional, bool? batch = null)
        {
            this.requests = requests;
            this.transactional = transactional;
            this.batch = batch;
        }
    }
}