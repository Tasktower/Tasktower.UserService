using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security.Auth.Middleware
{
    public static class JWTMiddlewareExtension
    {
        public static IApplicationBuilder UseJWTMiddleware<TKeyAccessor>(this IApplicationBuilder app, 
            Action<JWTMiddlewareOptions<TKeyAccessor>> configureOptions)
            where TKeyAccessor : class
        {
            var options = new JWTMiddlewareOptions<TKeyAccessor>();
            configureOptions(options);

            return app.UseMiddleware<JwtMiddleware<TKeyAccessor>>(options);
        }
    }
}
