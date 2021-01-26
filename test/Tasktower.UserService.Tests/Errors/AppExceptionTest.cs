using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tasktower.UserService.Dtos.Errors;
using Tasktower.UserService.Errors;
using Xunit;

namespace Tasktower.UserService.Tests.Errors
{
    public class AppExceptionTest
    {
        [Fact]
        public void CreateAppException_BadRequestMatches_WhenBadRequestImputted()
        {
            var appException = APIException.Create(APIException.Code.BAD_REQUEST);
            Assert.Equal(500, appException.StatusCode);
            Assert.Equal(APIException.Code.BAD_REQUEST, appException.ErrorCode);
        }

        [Fact]
        public void CreateAppException_AppPayloadMatches_WhenBadRequestImputted()
        {
            var appException = APIException.Create(APIException.Code.BAD_REQUEST);
            var payload = appException.Payload;
            Assert.Equal(500, payload.StatusCode);
            Assert.Equal(APIException.Code.BAD_REQUEST, payload.ErrorCode);
            Assert.Equal("Bad Request, Something went wrong", payload.Message);
        }

        [Fact]
        public void JSONSerializeAppErrorPayload_JSONConvertable_FromAppErrorPayloadJson()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var originalJson = "{\"statusCode\":500,\"errorCode\":\"BAD_REQUEST\",\"message\":\"Bad Request, Something went wrong\"}";
            var payload = JsonSerializer.Deserialize<APIErrorDto>(originalJson, options);
            var newJson = JsonSerializer.Serialize(payload, options);
            Assert.Equal(500, payload.StatusCode);
            Assert.Equal(APIException.Code.BAD_REQUEST, payload.ErrorCode);
            Assert.Equal("Bad Request, Something went wrong", payload.Message);
            Assert.Equal(originalJson, newJson);
        }
    }
}
