using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen
{
    public class repeat
    {
        public every every { get; set; }
        public int occurrences { get; set; }
        public DateTime endBy { get; set; }

        public repeat() { }

    }
}