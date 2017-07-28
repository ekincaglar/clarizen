using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class Currency
    {
        public double value { get; set; }
        public string currency { get; set; }

        public Currency(double value, string currency)
        {
            this.value = value;
            this.currency = currency;
        }
    }
}