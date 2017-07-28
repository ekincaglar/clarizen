using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class retrieveMultiple
    {
        public string[] fields { get; set; }
        public string[] ids { get; set; }

        public retrieveMultiple(string[] fields, string[] ids)
        {
            this.fields = fields;
            this.ids = ids;
        }
    }
}