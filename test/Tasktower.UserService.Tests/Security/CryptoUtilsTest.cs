using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.UserService.BusinessService;
using Tasktower.UserService.Security;
using Xunit;

namespace Tasktower.UserService.Tests.Security
{
    public class CryptoUtilsTest
    {

        [Fact]
        public void GenerateRandomBytes_DifferentBytes_WhenTwoBytesAreGenerated()
        {
            byte[] salt1 = CryptoUtils.GenerateRandomBytes(size: 32);
            byte[] salt2 = CryptoUtils.GenerateRandomBytes(size: 32);
            Assert.Equal(32, salt1.Length);
            Assert.Equal(32, salt2.Length);
            Assert.NotEqual(salt1, salt2);
        }

        [Fact]
        public void PasswordHash_PasswordValidationSucceeds_WhenSamePasswordUsedForValidationOnHash()
        {
            string originalPassword = "mypassword";
            byte[] salt = CryptoUtils.GenerateRandomBytes(size: 32);
            byte[] passwordHash = CryptoUtils.CreatePasswordHash(originalPassword, salt);
            Assert.True(CryptoUtils.VerifyPassword(originalPassword, passwordHash, salt));
        }

        [Fact(Skip = "Inconsistent timing")]
        public void PasswordHash_SuccessfulValidationLastsNoLessThan250MS_WhenSamePasswordUsedForValidationOnHash()
        {
            // Tests if password hashing is secure against brute force attacks
            // Validation should generally last at least 250 seconds
            string originalPassword = "mypassword";
            byte[] salt = CryptoUtils.GenerateRandomBytes(size: 32);
            byte[] passwordHash = CryptoUtils.CreatePasswordHash(originalPassword, salt);
            var timer = Stopwatch.StartNew();
            var result = CryptoUtils.VerifyPassword(originalPassword, passwordHash, salt);
            timer.Stop();
            Assert.True(timer.ElapsedMilliseconds >= 250);
            Assert.True(result);
        }

        [Fact]
        public void PasswordHash_PasswordValidationFails_WhenDifferentPasswordUsedForValidationOnHash()
        {
            string originalPassword = "mypassword";
            byte[] salt = CryptoUtils.GenerateRandomBytes(size: 32);
            byte[] passwordHash = CryptoUtils.CreatePasswordHash(originalPassword, salt);
            string wrongPassword = "wrongPassword";
            Assert.False(CryptoUtils.VerifyPassword(wrongPassword, passwordHash, salt));
        }

        [Fact]
        public void PasswordHash_PasswordValidationFails_WhenDifferentSaltUsedForValidationOnHash()
        {
            string originalPassword = "mypassword";
            byte[] salt = CryptoUtils.GenerateRandomBytes(size: 32);
            byte[] passwordHash = CryptoUtils.CreatePasswordHash(originalPassword, salt);
            byte[] wrongSalt = CryptoUtils.GenerateRandomBytes(size: 32);
            Assert.False(CryptoUtils.VerifyPassword(originalPassword, passwordHash, wrongSalt));
        }
    }
}
