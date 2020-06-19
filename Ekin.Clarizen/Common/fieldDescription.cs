using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Ekin.Clarizen
{
    public class FieldDescription
    {
        public string Name { get; set; }

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
        public string PresentationType { get; set; }

        public string Label { get; set; }

        public object DefaultValue { get; set; }

        public bool System { get; set; }

        public bool Calculated { get; set; }

        public bool Nullable { get; set; }

        public bool CreateOnly { get; set; }

        public bool Updateable { get; set; }


        [XmlElement("internal")]
        [JsonProperty("internal")]
        public bool Internal { get; set; }

        public bool Custom { get; set; }

        public bool Visible { get; set; }

        public int DecimalPlaces { get; set; }

        public bool Filterable { get; set; }

        public bool Sortable { get; set; }

        public int MaxLength { get; set; }

        public string[] Flags { get; set; }

        public string[] ReferencedEntities { get; set; }

        public FieldDescription() { }
    }
}