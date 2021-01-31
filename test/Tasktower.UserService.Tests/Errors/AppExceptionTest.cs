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
            var apiException = APIException.Create(APIException.Code.INTERNAL_SERVER_ERROR);
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, apiException.StatusCode);
            Assert.Equal(APIException.Code.INTERNAL_SERVER_ERROR, apiException.ErrorCode);
        }

        [Fact]
        public void CreateAppExceptionFromMultipleMultipleExceptionsMatch_WhenTwoMultipleExceptionsInputted()
        {
            ICollection<APIException> exceptions = new List<APIException>
            {
                APIException.Create(APIException.Code.ACCOUNT_ALREADY_EXISTS),
                APIException.Create(APIException.Code.ACCOUNT_FAILED_TO_CREATE)
            };
            var apiException = APIException.CreateFromMultiple(exceptions);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, apiException.StatusCode);
            Assert.Equal(APIException.Code.MULTIPLE_EXCEPTIONS_FOUND, apiException.ErrorCode);
            Assert.Equal(2, apiException.MultipleErrors.Count());
            Assert.Contains(apiException.MultipleErrors, e => 
                e.ErrorCode == APIException.Code.ACCOUNT_ALREADY_EXISTS);
            Assert.Contains(apiException.MultipleErrors, e => 
                e.ErrorCode == APIException.Code.ACCOUNT_FAILED_TO_CREATE);
        }
    }
}
