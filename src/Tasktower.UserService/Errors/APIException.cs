using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Dtos.Errors;

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
                Code.ACCOUNT_NOT_FOUND => new APIException(code, 400, "Account not found", args),
                Code.ACCOUNT_FAILED_TO_CREATE => new APIException(code, 400, "Account failed to create", args),
                Code.BAD_REQUEST => new APIException(code, 500, "Bad Request, Something went wrong", args),
                _ => new APIException(Code.BAD_REQUEST, 500, "Bad Request, Something went wrong", args),
            };
        }

        private APIException(Code code,
            int statusCode,
            string messageFormat,
            params string[] args) : base(string.Format(messageFormat, args))
        {
            ErrorCode = code;
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }

        public Code ErrorCode { get; set; }

        public APIErrorDto Payload { get 
            {
                return new APIErrorDto
                {
                    Message = Message,
                    StatusCode = StatusCode,
                    ErrorCode = ErrorCode,
                };
            }  
        }
    }
}
