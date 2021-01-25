using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tasktower.UserService.Errors;

namespace Tasktower.UserService.Errors
{
    public class AppErrorPayload
    {
        private class ErrorCodeConverter : JsonConverter<AppException.Code>
        {
            public override AppException.Code Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                try
                {
                    return (AppException.Code)Enum.Parse(typeof(AppException.Code), reader.GetString());
                }
                catch (Exception)
                {
                    throw new NotSupportedException("Not an app error code");
                }

            }

            public override void Write(
                Utf8JsonWriter writer,
                AppException.Code code,
                JsonSerializerOptions options) =>
                    writer.WriteStringValue(code.ToString());
        }

        public int StatusCode { get; set; }

        [JsonConverter(typeof(ErrorCodeConverter))]
        public AppException.Code ErrorCode { get; set; }
        public string Message { get; set; }
    }
}
