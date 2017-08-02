using Ekin.Clarizen.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Metadata.Result
{
    public class getSystemSettingsValues
    {
        /// <summary>
        /// Array of objects representing the values of each system setting. The value type (e.g. boolean, int etc.) depends on the system setting
        /// </summary>
        public object[] values { get; set; }
    }
}