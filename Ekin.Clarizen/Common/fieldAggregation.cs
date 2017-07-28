using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    /// <summary>
    /// Define an aggregation to perform in an aggregate query 
    /// </summary>
    public class fieldAggregation
    {
        /// <summary>
        /// Type of aggregate function to perform (e.g. Count, Sum, etc.)
        /// </summary>
        public string function { get; set; }
        /// <summary>
        /// Name of a field to perform this function on
        /// </summary>
        public string fieldName { get; set; }
        /// <summary>
        /// (Optional) A name that will represent the result of this function. If this isn't provided, a default name will be generated (e.g. Max_PercentCompleted when performing a Max function on the PercentCompleted field)
        /// </summary>
        public string alias { get; set; }

        public fieldAggregation(string function, string fieldName, string alias)
        {
            this.function = function;
            this.fieldName = fieldName;
            this.alias = alias;
        }
    }
}