using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class cZQLCondition : ICondition
    {
        public string _type { get { return "cZQLCondition"; } }

        /// <summary>
        /// The condition text (e.g. PercentCompleted>50 AND DueDate>:dateParam) You can use bind parameters in the condition text and fill the parameter values using Parameters property
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// Values for bind parameters
        /// </summary>
        public parameters parameters { get; set; }

        public cZQLCondition(string text, parameters parameters)
        {
            this.text = text;
            this.parameters = parameters;
        }

        public cZQLCondition(string text)
        {
            this.text = text;
        }
    }
}