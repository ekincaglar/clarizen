using System;

namespace Ekin.Clarizen
{
    public class EntityId
    {
        public string id { get; set; }

        public EntityId()
        {
        }

        public EntityId(string Id)
        {
            id = Id;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var item = obj as EntityId;

            if (object.ReferenceEquals(id, null) ||
                object.ReferenceEquals(item, null) ||
                object.ReferenceEquals(item.id, null))
            {
                return false;
            }

            return id.Equals(item.id, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}