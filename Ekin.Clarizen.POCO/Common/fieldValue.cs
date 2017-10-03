using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class fieldValue
    {
        public string fieldName { get; set; }
        public string value { get; set; }

        public fieldValue() { }

        public fieldValue(string fieldName, string value)
        {
            this.fieldName = fieldName;
            this.value = value;
        }
    }
}