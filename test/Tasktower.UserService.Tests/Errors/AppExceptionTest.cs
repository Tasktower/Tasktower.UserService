using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
    }
}
