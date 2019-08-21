using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ekin.Clarizen.POCO.Responses.Data
{
    public class getMissingTimesheets
    {
        public List<dayOfMissingTimesheets> missingTimesheets { get; set; }

        public getMissingTimesheets() { }
    }
}
