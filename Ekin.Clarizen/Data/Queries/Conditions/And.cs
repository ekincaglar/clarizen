using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class And : ICondition
    {
        public string _type { get { return "and"; } }
        public ICondition[] and { get; set; }

        public And(ICondition[] and)
        {
            this.and = and;
        }

    }
}