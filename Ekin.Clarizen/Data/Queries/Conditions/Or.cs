using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class Or : ICondition
    {
        public string _type { get { return "or"; } }
        public ICondition[] or { get; set; }

        public Or(ICondition[] or)
        {
            this.or = or;
        }

    }
}