﻿using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class compare : ICondition
    {
        public string _type { get { return "compare"; } }
        public IExpression leftExpression { get; set; }
        [XmlElement("operator")]
        [JsonProperty("operator")]
        public string Operator { get; set; }
        public IExpression rightExpression { get; set; }

        public compare(IExpression leftExpression, Operator Operator, IExpression rightExpression)
        {
            this.leftExpression = leftExpression;
            this.Operator = Operator.ToEnumString();
            this.rightExpression = rightExpression;
        }

    }
}