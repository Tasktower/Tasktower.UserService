using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Tasktower.UserService.Security.AuthData
{
    public class UserJWTClaims
    {
        public Guid UserID { get; set; }
        public IEnumerable<Role> Roles {get; set;}
        public bool EmailVerified { get; set; }

        public string XSRFToken { get; set; }

        public IEnumerable<Claim> ToJWTClaimsIEnumerable(DateTime issuedAt) 
        {
            return new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(nameof(UserID), UserID.ToString()),
                new Claim(nameof(Roles), string.Join(",", Roles)),
                new Claim(nameof(EmailVerified), EmailVerified.ToString(), ClaimValueTypes.Boolean),
                new Claim(nameof(XSRFToken), XSRFToken),
            };
        }

        public static bool TryParseFromJWT(string token, JwtSecurityTokenHandler handler, out UserJWTClaims claims)
        {
            claims = null;
            handler ??= new JwtSecurityTokenHandler();
            var payload = handler.ReadJwtToken(token).Payload;

            if (!bool.TryParse(payload.GetValueOrDefault(nameof(EmailVerified)).ToString(), out bool emailVerified))
            {
                return false;
            }

            if (!Guid.TryParse(payload.GetValueOrDefault(nameof(UserID)).ToString(), out Guid userID)) 
            {
                return false;
            }

            string xsrfToken = payload.GetValueOrDefault(nameof(XSRFToken)).ToString();

            var roles = (payload.GetValueOrDefault(nameof(Roles))).ToString().Split(",").AsParallel()
                .Select(r =>
                {
                    if (!Enum.TryParse(r, out Role role))
                    {
                        return Role.DEFAULT;
                    }
                    return role;
                }).Where(r => r != Role.DEFAULT);

            claims = new UserJWTClaims
            {
                EmailVerified = emailVerified,
                Roles = roles,
                UserID = userID,
                XSRFToken = xsrfToken,
            };
            return true;
        }
    }
}
