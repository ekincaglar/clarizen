using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Metadata.Request
{
    public class describeEntities
    {
        /// <summary>
        /// The types of entities to describe
        /// </summary>
        public string[] typeNames { get; set; }

        public describeEntities(string[] typeNames)
        {
            this.typeNames = typeNames;
        }
    }
}