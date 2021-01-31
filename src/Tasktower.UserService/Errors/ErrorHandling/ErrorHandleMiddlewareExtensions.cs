using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Errors.ErrorHandling
{
    public static class ErrorHandleMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustonErrorHandler(this IApplicationBuilder builder, ErrorHandleMiddlewareOptions opt)
        {
            return builder.UseMiddleware<ErrorHandeMiddleware>(opt);
        }
    }
}
