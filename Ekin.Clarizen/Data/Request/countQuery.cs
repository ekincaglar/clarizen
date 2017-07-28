using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class countQuery
    {
        public IQuery query { get; set; }

        public countQuery(IQuery query)
        {
            this.query = query;
        }
    }
}