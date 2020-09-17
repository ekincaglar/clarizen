using Ekin.Clarizen.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Ekin.Clarizen
{
    public class EntityId: IEntity
    {
        private string _id { get; set; }
        private string _id_value { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                _id_value = null; // This will get calculated the first time Id_value property is accessed
            }
        }

        [JsonIgnore]
        public string Id_value
        {
            get 
            {
                if (_id_value != null) return _id_value;

                // Find the text after the second slash in the Id and set it to the Id_value property
                if (!string.IsNullOrWhiteSpace(_id))
                {
                    string idToOperateOn = _id;

                    // For Fully Qualified Ids (FQIDs) in a multi-org environment, ignore the Organization part of the Id
                    if (idToOperateOn.StartsWith("/Organization/"))
                    {
                        idToOperateOn = GetValueAfterSecondSlash(idToOperateOn);
                    }

                    _id_value = GetValueAfterSecondSlash(idToOperateOn);
                }

                return _id_value;
            }
        }

        public EntityId() { }

        public EntityId(string Id)
        {
            this.Id = Id;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            EntityId item = obj as EntityId;

            if (Id is null || item is null || item.Id is null)
            {
                return false;
            }

            return Id.Equals(item.Id, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public List<string> GetMultiSelectPickListIds()
        {
            if (string.IsNullOrEmpty(Id_value)) return new List<string>();
            return Id_value.Split(';').Select(str => str.Trim()).ToList();
        }

        private string GetValueAfterSecondSlash(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            // Apologies for the lack of syntactic sugar but the below algorithm is, sadly, faster than a LINQ or a Regex solution
            int firstSlash = value.IndexOf('/');
            if (firstSlash >= 0)
            {
                int secondSlash = value.IndexOf('/', firstSlash + 1);
                if (secondSlash >= 0)
                {
                    return value.Length > secondSlash + 1 ? value.Substring(secondSlash + 1) : string.Empty;
                }
            }

            return string.Empty;
        }
    }
}