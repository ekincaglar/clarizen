using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class objects_delete
    {
        public string id { get; set; }

        public objects_delete(string id)
        {
            this.id = id;
        }
    }
    
}