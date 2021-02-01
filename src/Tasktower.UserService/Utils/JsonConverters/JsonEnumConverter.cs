using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tasktower.UserService.Utils.JsonConverters
{
    public class JsonEnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), reader.GetString());
            }
            catch (Exception)
            {
                throw new NotSupportedException($"Not a {typeof(T).Name}");
            }

        }

        public override void Write(Utf8JsonWriter writer, T code, JsonSerializerOptions options) => 
            writer.WriteStringValue(code.ToString());

    }
}
