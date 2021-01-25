using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Errors;

namespace Tasktower.UserService.Errors
{
    public class AppException : Exception
    {
        public enum Code
        {
            BAD_REQUEST,
            ACCOUNT_NOT_FOUND,
            ACCOUNT_FAILED_TO_CREATE
        }

        public static AppException CreateAppException(Code code, params string[] args) 
        {
            return code switch
            {
                Code.ACCOUNT_NOT_FOUND => new AppException(code, 400, "Account not found", args),
                Code.ACCOUNT_FAILED_TO_CREATE => new AppException(code, 400, "Account failed to create", args),
                Code.BAD_REQUEST => new AppException(code, 500, "Bad Request, Something went wrong", args),
                _ => new AppException(Code.BAD_REQUEST, 500, "Bad Request, Something went wrong", args),
            };
        }

        private AppException(Code code,
            int statusCode,
            string messageFormat,
            params string[] args) : base(string.Format(messageFormat, args))
        {
            ErrorCode = code;
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }

        public Code ErrorCode { get; set; }

        public AppErrorPayload Payload { get 
            {
                return new AppErrorPayload
                {
                    Message = Message,
                    StatusCode = StatusCode,
                    ErrorCode = ErrorCode,
                };
            }  
        }
    }
}
