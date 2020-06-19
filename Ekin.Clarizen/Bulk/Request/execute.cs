using System.Collections.Generic;

namespace Ekin.Clarizen.Bulk.Request
{
    public class Execute
    {
        /// <summary>
        /// Array of Request objects representing individual API calls
        /// </summary>
        public List<Clarizen.Request> Requests { get; set; }
        public bool Transactional { get; set; }
        public bool? Batch { get; set; }

        public Execute(List<Clarizen.Request> requests, bool transactional, bool? batch = null)
        {
            this.Requests = requests;
            this.Transactional = transactional;
            this.Batch = batch;
        }
    }
}