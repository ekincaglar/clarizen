using Newtonsoft.Json;
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


    public class EntityIdConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is EntityId)
            {
                writer.WriteValue(((EntityId)value).id);
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(EntityId))
            {
                return serializer.Deserialize<EntityId>(reader);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}