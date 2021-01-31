using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Tasktower.UserService.Errors
{
    public class APIException : Exception
    {
        public enum Code
        {
            INTERNAL_SERVER_ERROR,
            ACCOUNT_NOT_FOUND,
            ACCOUNT_FAILED_TO_CREATE,
            ACCOUNT_ALREADY_EXISTS,
            MULTIPLE_EXCEPTIONS_FOUND
        }

        public static APIException CreateFromMultiple(IEnumerable<APIException> multipleExceptions)
        {
            var apiEx = Create(Code.MULTIPLE_EXCEPTIONS_FOUND);
            apiEx.MultipleErrors = multipleExceptions;
            return apiEx;
        }

        public static APIException Create(Code code, params string[] args) 
        {
            return code switch
            {
                Code.ACCOUNT_ALREADY_EXISTS => new APIException(code, HttpStatusCode.BadRequest, "Account already exists", args),
                Code.ACCOUNT_NOT_FOUND => new APIException(code, HttpStatusCode.BadRequest, "Account not found", args),
                Code.ACCOUNT_FAILED_TO_CREATE => new APIException(code, HttpStatusCode.BadRequest, "Account failed to create", args),
                Code.MULTIPLE_EXCEPTIONS_FOUND => new APIException(code, HttpStatusCode.BadRequest, "Multiple exceptions found", args),
                Code.INTERNAL_SERVER_ERROR => new APIException(code, HttpStatusCode.InternalServerError, "Something went wrong", args),
                _ => new APIException(Code.INTERNAL_SERVER_ERROR, HttpStatusCode.InternalServerError, "Something went wrong", args),
            };
        }

        private APIException(Code code, HttpStatusCode statusCode, string messageFormat, params string[] args) 
            : base(string.Format(messageFormat, args))
        {
            ErrorCode = code;
            StatusCode = statusCode;
        }

        public IEnumerable<APIException> MultipleErrors { get; internal set; } = null; 

        public HttpStatusCode StatusCode { get; internal set; }

        public Code ErrorCode { get; internal set; }
    }
}
