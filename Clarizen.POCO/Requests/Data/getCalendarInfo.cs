using System.Xml.Serialization;
using Newtonsoft.Json;

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