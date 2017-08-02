using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ekin.Clarizen.Data.Request
{
    public class getTemplateDescriptions
    {
        public string typeName { get; set; }

        public getTemplateDescriptions(string typeName)
        {
            this.typeName = typeName;
        }
    }
}