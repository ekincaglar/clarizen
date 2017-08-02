using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class query
    {
        /// <summary>
        /// The CZQL Query to perform
        /// </summary>
        public string q { get; set; }
        /// <summary>
        /// Values for bind parameters
        /// </summary>
        public object[] parameters { get; set; }
        /// <summary>
        /// paging setting for the query
        /// </summary>
        public paging paging { get; set; }

        public query(string q, object[] parameters, paging paging)
        {
            this.q = q;
            this.parameters = parameters;
            this.paging = paging;
        }

        public query(string q, paging paging)
        {
            this.q = q;
            this.paging = paging;
        }

        public query(string q)
        {
            this.q = q;
        }

    }
}