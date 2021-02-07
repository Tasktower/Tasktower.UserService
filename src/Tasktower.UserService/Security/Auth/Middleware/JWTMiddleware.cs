using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Security.Auth.AuthData;

namespace Tasktower.UserService.Security.Auth.Middleware
{
    public class JwtMiddleware<TKeyAccessor> where TKeyAccessor : class
    {
        private readonly RequestDelegate _next;
        private readonly JWTMiddlewareOptions<TKeyAccessor> _options;

        public JwtMiddleware(RequestDelegate next, JWTMiddlewareOptions<TKeyAccessor> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context, TKeyAccessor accessor)
        {
            string accessToken = context.Request.Cookies["ACCESS-TOKEN"];
            string xsrfToken = context.Request.Headers["X-XSRF-TOKEN"];
            context.Items["XSRFToken"] = xsrfToken ?? "";

            if (accessToken != null)
                await attachUserClaimsToContext(context, accessor, accessToken);

            await _next(context);
        }

        private async Task attachUserClaimsToContext(HttpContext context, TKeyAccessor accessor, string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                string kid = handler.ReadJwtToken(token).Header.GetValueOrDefault("kid")?.ToString() ?? "";
                string pem = await _options.KeyRetrieverAsync.Invoke(kid, accessor);


                using RSACryptoServiceProvider rsa = CryptoUtils.RSAFromPublicPem(pem);

                var rsaSecurityKey = new RsaSecurityKey(rsa) { KeyId = kid };


                if (!CryptoUtils.TryParseAndValidateJWTToken(token, rsaSecurityKey, handler, out UserAuthData userData,
                    UserAuthData.TryParseFromJWTPayload))
                {
                    return;
                }

                context.Items["UserData"] = userData;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
