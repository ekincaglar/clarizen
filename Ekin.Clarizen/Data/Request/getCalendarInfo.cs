using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Ekin.Clarizen.Data.Request
{
    public class GetCalendarInfo
    {
        [XmlElement("userId")]
        [JsonProperty("userId")]
        public string Id { get; set; }

        public GetCalendarInfo(string id)
        {
            Id = id;
        }
    }
}