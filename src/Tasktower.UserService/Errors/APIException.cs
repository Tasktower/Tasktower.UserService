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
            BAD_REQUEST,
            ACCOUNT_NOT_FOUND,
            ACCOUNT_FAILED_TO_CREATE
        }

        public static APIException Create(Code code, params string[] args) 
        {
            return code switch
            {
                Code.ACCOUNT_NOT_FOUND => new APIException(code, HttpStatusCode.BadRequest, "Account not found", args),
                Code.ACCOUNT_FAILED_TO_CREATE => new APIException(code, HttpStatusCode.BadRequest, "Account failed to create", args),
                Code.BAD_REQUEST => new APIException(code, HttpStatusCode.InternalServerError, "Bad Request, Something went wrong", args),
                _ => new APIException(Code.BAD_REQUEST, HttpStatusCode.InternalServerError, "Bad Request, Something went wrong", args),
            };
        }

        private APIException(Code code, HttpStatusCode statusCode, string messageFormat, params string[] args) 
            : base(string.Format(messageFormat, args))
        {
            ErrorCode = code;
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }

        public Code ErrorCode { get; set; }
    }
}
