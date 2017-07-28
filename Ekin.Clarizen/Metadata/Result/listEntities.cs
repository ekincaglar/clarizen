using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Metadata.Result
{
    public class listEntities
    {
        public string[] typeNames { get; set; }

        public void SortTypeNames()
        {
            Array.Sort(typeNames);
        }
    }
}