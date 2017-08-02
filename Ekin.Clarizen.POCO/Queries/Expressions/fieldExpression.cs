using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents an object field 
    /// </summary>
    public class fieldExpression : IExpression
    {
        public string _type { get { return "fieldExpression"; } }

        /// <summary>
        /// The name of the field this expression represents (e.g. "Name","PercentCompleted","Manager" etc.)
        /// </summary>
        public string fieldName { get; set; }

        public fieldExpression(string fieldName)
        {
            this.fieldName = fieldName;
        }
    }
}