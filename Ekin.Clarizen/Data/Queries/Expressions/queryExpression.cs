using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents a query. Used in IN conditions 
    /// </summary>
    public class queryExpression : IExpression
    {
        public string _type { get { return "queryExpression"; } }

        /// <summary>
        /// The actual query
        /// </summary>
        public IQuery query { get; set; }

        public queryExpression(IQuery query)
        {
            this.query = query;
        }
    }
}