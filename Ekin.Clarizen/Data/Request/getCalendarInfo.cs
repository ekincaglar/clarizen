using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ekin.Clarizen.Data.Request
{
    public class getCalendarInfo
    {
        [XmlElement("userId")]
        [JsonProperty("userId")]
        public string id { get; set; }

        public getCalendarInfo(string id)
        {
            this.id = id;
        }
    }
}