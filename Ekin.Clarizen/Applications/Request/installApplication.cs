using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Applications.Request
{
    public class installApplication
    {
        public string applicationId { get; set; }
        public bool autoEnable { get; set; }

        public installApplication(string applicationId, bool autoEnable)
        {
            this.applicationId = applicationId;
            this.autoEnable = autoEnable;
        }
    }
}