using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents a constant value (e.g. 1, 'Joe', true, null etc.) 
    /// </summary>
    public class constantExpression : IExpression
    {
        public string _type { get { return "constantExpression"; } }

        /// <summary>
        /// The expression value
        /// </summary>
        public object value { get; set; }

        public constantExpression(object value)
        {
            this.value = value;
        }
    }
}