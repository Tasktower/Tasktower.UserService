using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tasktower.UserService.Errors;

namespace Tasktower.UserService.Dtos.Errors
{
    public class APIErrorDto
    {
        private class ErrorCodeConverter : JsonConverter<APIException.Code>
        {
            public override APIException.Code Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                try
                {
                    return (APIException.Code)Enum.Parse(typeof(APIException.Code), reader.GetString());
                }
                catch (Exception)
                {
                    throw new NotSupportedException("Not an app error code");
                }

            }

            public override void Write(
                Utf8JsonWriter writer,
                APIException.Code code,
                JsonSerializerOptions options) =>
                    writer.WriteStringValue(code.ToString());
        }

        public int StatusCode { get; set; }

        [JsonConverter(typeof(ErrorCodeConverter))]
        public APIException.Code ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
