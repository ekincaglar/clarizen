using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Queries.Conditions
{
    public class Not : ICondition
    {
        public string _type { get { return "not"; } }
        public ICondition not { get; set; }

        public Not(ICondition not)
        {
            this.not = not;
        }

    }
}