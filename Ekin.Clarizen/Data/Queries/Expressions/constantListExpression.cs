﻿using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Expressions
{
    /// <summary>
    /// An expression which represents array of constant values. Used in IN conditions 
    /// </summary>
    public class constantListExpression : IExpression
    {
        public string _type { get { return "constantListExpression"; } }

        /// <summary>
        /// The value list
        /// </summary>
        public object[] value { get; set; }

        public constantListExpression(object[] value)
        {
            this.value = value;
        }
    }
}