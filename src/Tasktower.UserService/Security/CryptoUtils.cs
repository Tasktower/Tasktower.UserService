using CryptSharp.Utility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Tasktower.UserService.Security
{
    public class CryptoUtils
    {
        public static byte[] GenerateRandomBytes(int size = 32)
        {
            byte[] bytes = new byte[size];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(bytes);
            }
            return bytes;
        }

        public static byte[] CreateSHA256Hash(byte[] refreshTokenBytes, byte[] refreshTokenSalt)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(refreshTokenBytes.Concat(refreshTokenSalt).ToArray());
        }

        public static bool VerifySHA256(byte[] candidateRefreshTokenBytes, byte[] realRefreshTokenHash, byte[] refreshTokenSalt)
        {
            using var sha = SHA256.Create();
            var candidateHash = sha.ComputeHash(candidateRefreshTokenBytes.Concat(refreshTokenSalt).ToArray());

            return candidateHash.SequenceEqual(realRefreshTokenHash);
        }

        public static byte[] CreatePasswordHash(string password, byte[] passwordSalt)
        {
            var passwordBytes = System.Text.Encoding.Unicode.GetBytes(password);
            byte[] hash = new byte[128];
            SCrypt.ComputeKey(passwordBytes, passwordSalt, 32768, 8, 1, 1, hash); // 32768
            return hash;
        }

        public static bool VerifyPassword(string expectedPwd, byte[] givenPasswordHash, byte[] passwordSalt)
        {
            var expectedPwdHash = CreatePasswordHash(expectedPwd, passwordSalt);
            return Enumerable.SequenceEqual(expectedPwdHash, givenPasswordHash);
        }
        public static string CreateRSAPublicPem(RSACryptoServiceProvider rsa)
        {
            return "-----BEGIN RSA PUBLIC KEY-----\n" +
                $"{Convert.ToBase64String(rsa.ExportRSAPublicKey())}\n" +
                "-----END RSA PUBLIC KEY-----";
        }

        public static RSACryptoServiceProvider RSAFromPublicPem(string pem)
        {
            var pubRSA = new RSACryptoServiceProvider();
            pubRSA.ImportFromPem(pem);
            return pubRSA;
        }

        public static string CreateJWTToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims, DateTime? notBefore = null, 
            DateTime? expires = null)
        {
            var jwt = new JwtSecurityToken(
                audience: JWTIdentityConstants.JwtAudience,
                issuer: JWTIdentityConstants.JwtIssuer,
                claims: claims,
                notBefore: notBefore,
                expires: expires,
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        public delegate bool ActionJwtParser<T>(string token, JwtSecurityTokenHandler handler, out T claims);

        public static bool TryParseAndValidateJWTToken<TClaims>(string token, SecurityKey key, out TClaims claims, 
            ActionJwtParser<TClaims> parser) 
            where TClaims : class
        {
            var handler = new JwtSecurityTokenHandler();

            if(!parser.Invoke(token, handler, out claims))
            {
                return false;
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JWTIdentityConstants.JwtIssuer,
                ValidAudience = JWTIdentityConstants.JwtAudience,
                IssuerSigningKey = key
            };

            try
            {
                handler.ValidateToken(token, validationParameters, out var validatedSecurityToken);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
