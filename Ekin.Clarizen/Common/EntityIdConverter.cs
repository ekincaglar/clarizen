using Newtonsoft.Json;
using System;

namespace Ekin.Clarizen
{
    public class EntityIdConverter : JsonConverter
    {

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is EntityId)
            {
                writer.WriteValue(((EntityId)value).Id);
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                if (objectType == typeof(EntityId))
                {
                    if (reader.Value != null && !string.IsNullOrEmpty(reader.Value.ToString()))
                    {
                        return new EntityId(reader.Value?.ToString());
                    }
                    else
                    {
                        var result = serializer.Deserialize<EntityId>(reader);
                        return result;
                    }
                }
            }
            catch
            {
                if (reader.Value != null && !string.IsNullOrEmpty(reader.Value.ToString()))
                {
                    return new EntityId(reader.Value?.ToString());
                }
                return null;
            }

            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
