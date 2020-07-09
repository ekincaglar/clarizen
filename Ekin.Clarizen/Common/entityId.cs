using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ekin.Clarizen
{
    public class EntityId
    {
        private string _id { get; set; }
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;

                // Find the text after the second slash in the Id and set it to the Id_value property
                if (!string.IsNullOrWhiteSpace(_id))
                {
                    // Apologies for the lack of syntactic sugar but the below algorithm is, sadly, faster than a LINQ or a Regex solution
                    int firstSlash = _id.IndexOf('/');
                    if (firstSlash >= 0)
                    {
                        int secondSlash = _id.IndexOf('/', firstSlash + 1);
                        if (secondSlash >= 0)
                        {
                            Id_value = _id.Length > secondSlash + 1 ? _id.Substring(secondSlash + 1) : string.Empty;
                        }
                    }
                }
            }
        }

        [JsonIgnore]
        public string Id_value { get; private set; }

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
    }
}