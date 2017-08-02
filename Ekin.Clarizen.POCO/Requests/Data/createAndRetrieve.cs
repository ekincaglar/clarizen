using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class createAndRetrieve
    {
        public object entity { get; set; }
        public string[] fields { get; set; }

        public createAndRetrieve(object entity, string[] fields)
        {
            this.entity = entity;
            this.fields = fields;
        }
    }
}