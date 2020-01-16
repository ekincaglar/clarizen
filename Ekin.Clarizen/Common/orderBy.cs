using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class orderBy
    {
        public string fieldName { get; set; }
        /// <summary>
        /// Possible values: Ascending | Descending
        /// </summary>
        public string order { get; set; }

        public orderBy() { }

        public orderBy(string fieldName, string order)
        {
            this.fieldName = fieldName;
            this.order = order;
        }
    }
}