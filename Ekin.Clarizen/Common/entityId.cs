using Newtonsoft.Json;
using System;

namespace Ekin.Clarizen
{
    public class EntityId
    {
        public string Id { get; set; }

        [JsonIgnore]
        public string Id_value
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Id) && Id.Contains("/") ? Id.Substring(Id.LastIndexOf("/") + 1) : Id;
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

    }



}