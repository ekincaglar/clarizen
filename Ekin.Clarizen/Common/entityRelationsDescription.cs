using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class entityRelationsDescription
    {
        public string typeName { get; set; }
        public relationDescription[] relations { get; set; }

        public entityRelationsDescription() { }

        public void SortRelations()
        {
            Array.Sort(relations, delegate (relationDescription x, relationDescription y) { return x.name.CompareTo(y.name); });
        }
    }
}