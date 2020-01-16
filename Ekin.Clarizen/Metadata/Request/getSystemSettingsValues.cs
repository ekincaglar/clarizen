﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Metadata.Request
{
    public class getSystemSettingsValues
    {
        /// <summary>
        /// List of system setting names
        /// </summary>
        public string[] settings { get; set; }

        public getSystemSettingsValues(string[] settings)
        {
            this.settings = settings;
        }
    }
}