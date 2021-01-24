using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CryptSharp.Utility;

namespace Tasktower.UserService.BLL
{
    public class CryptoBLL : ICryptoBLL
    {
        public CryptoBLL() { }

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
            var hash = SCrypt.ComputeDerivedKey(passwordBytes, passwordSalt, 262144, 8, 1, null, 128);
            return hash;
        }

        public bool VerifyPasswordHash(string expectedPwd, byte[] givenPasswordHash, byte[] passwordSalt)
        {
            var expectedPwdHash = PasswordHash(expectedPwd, passwordSalt);
            return Enumerable.SequenceEqual(expectedPwdHash, givenPasswordHash);
        }

    }
}
