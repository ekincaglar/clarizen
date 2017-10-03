using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class relationDescription
    {
        public string name { get; set; }
        public string label { get; set; }
        public string roleLabel { get; set; }
        public bool readOnly { get; set; }
        public string linkTypeName { get; set; }
        public string relatedTypeName { get; set; }
        public string sourceFieldName { get; set; }

        public relationDescription() { }
    }
}