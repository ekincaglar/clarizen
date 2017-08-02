using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class entityDescription
    {
        public string typeName { get; set; }
        public fieldDescription[] fields { get; set; }
        public string[] validStates { get; set; }
        public string label { get; set; }
        public string labelPlural { get; set; }
        public string parentEntity { get; set; }
        public string displayField { get; set; }
        public bool disabled { get; set; }
        public relationDescription[] relations { get; set; }

        public void SortFields()
        {
            Array.Sort(fields, delegate (fieldDescription x, fieldDescription y) { return x.name.CompareTo(y.name); });
        }
    }
}