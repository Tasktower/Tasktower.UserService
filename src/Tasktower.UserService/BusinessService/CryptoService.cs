using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CryptSharp.Utility;
using Tasktower.UserService.Utils.DependencyInjection;

namespace Tasktower.UserService.BusinessService
{
    [BusinessService]
    public class CryptoService : ICryptoService
    {
        public CryptoService() { }

        public byte[] GeneratePasswordSalt()
        {
            byte[] salt = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with a random value.
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        public byte[] PasswordHash(string password, byte[] passwordSalt) {
            var passwordBytes = System.Text.Encoding.Unicode.GetBytes(password);
            byte[] hash = new byte[128];
            SCrypt.ComputeKey(passwordBytes, passwordSalt, 32768, 8, 1, 1, hash); // 32768
            return hash;
        }

        public bool VerifyPasswordHash(string expectedPwd, byte[] givenPasswordHash, byte[] passwordSalt)
        {
            var expectedPwdHash = PasswordHash(expectedPwd, passwordSalt);
            return Enumerable.SequenceEqual(expectedPwdHash, givenPasswordHash);
        }

    }
}
