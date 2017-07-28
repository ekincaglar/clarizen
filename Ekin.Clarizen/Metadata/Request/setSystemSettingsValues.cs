using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Metadata.Request
{
    public class setSystemSettingsValues
    {
        /// <summary>
        /// List of system setting values
        /// </summary>
        public fieldValue[] settings { get; set; }

        public setSystemSettingsValues(fieldValue[] settings)
        {
            this.settings = settings;
        }
    }
}