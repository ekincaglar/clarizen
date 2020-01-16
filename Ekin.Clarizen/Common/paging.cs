using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class paging
    {
        /// <summary>
        /// The record number to start retrieve from
        /// </summary>
        public int from { get; set; }
        /// <summary>
        /// Number of records to retrieve
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// When a query results is returned, indicates whether there are more records to fetch for this query
        /// </summary>
        public bool hasMore { get; set; }

        public paging() { }
    }
}