using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Ekin.Clarizen
{
    public class fieldDescription
    {
        public string name { get; set; }
        /// <summary>
        /// Possible values:
        /// Boolean | String | Integer | Long | Double | DateTime | Date | Entity | Duration | Money | MultiPickList | Binary
        /// </summary>
        [XmlElement("type")]
        [JsonProperty("type")]
        public string _type { get; set; }
        /// <summary>
        /// Possible values:
        /// Text | Numeric | Date | Checkbox | TextArea | Currency | Duration | ReferenceToObject | PickList | Url | Percent | RichText | MultiPickList | Other
        /// </summary>
        public string presentationType { get; set; }
        public string label { get; set; }
        public object defaultValue { get; set; }
        public bool system { get; set; }
        public bool calculated { get; set; }
        public bool nullable { get; set; }
        public bool createOnly { get; set; }
        public bool updateable { get; set; }
        [XmlElement("internal")]
        [JsonProperty("internal")]
        public bool _internal { get; set; }
        public bool custom { get; set; }
        public bool visible { get; set; }
        public int decimalPlaces { get; set; }
        public bool filterable { get; set; }
        public bool sortable { get; set; }
        public int maxLength { get; set; }
        public string[] flags { get; set; }
    }
}