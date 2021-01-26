using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.UserService.BLL;
using Xunit;

namespace Tasktower.UserService.Tests.BLL
{
    public class CryptoBLLTest : IDisposable
    {
        private readonly ICryptoBLL _cryptoBLL;
        public CryptoBLLTest() {
            _cryptoBLL = new CryptoBLL();
        }
        public void Dispose()
        {
            return;
        }

        [Fact]
        public void GeneratePasswordSalt_DifferentHash_WhenTwoHashesAreGenerated()
        {
            byte[] salt1 = _cryptoBLL.GeneratePasswordSalt();
            byte[] salt2 = _cryptoBLL.GeneratePasswordSalt();
            Assert.False(Enumerable.SequenceEqual(salt1, salt2));
        }

        [Fact]
        public void PasswordHash_PasswordValidationSucceeds_WhenSamePasswordUsedForValidationOnHash() 
        {
            string originalPassword = "mypassword";
            byte[] salt = _cryptoBLL.GeneratePasswordSalt();
            byte[] passwordHash = _cryptoBLL.PasswordHash(originalPassword, salt);
            Assert.True(_cryptoBLL.VerifyPasswordHash(originalPassword, passwordHash, salt));
        }

        [Fact]
        public void PasswordHash_SuccessfulValidationLastsNoLessThan250MS_WhenSamePasswordUsedForValidationOnHash()
        {
            // Tests if password hashing is secure against brute force attacks
            // Validation should generally last at least 250 seconds
            string originalPassword = "mypassword";
            byte[] salt = _cryptoBLL.GeneratePasswordSalt();
            byte[] passwordHash = _cryptoBLL.PasswordHash(originalPassword, salt);
            var timer = Stopwatch.StartNew();
            var result = _cryptoBLL.VerifyPasswordHash(originalPassword, passwordHash, salt);
            timer.Stop();
            Assert.True(timer.ElapsedMilliseconds >= 250);
            Assert.True(result);
        }

        [Fact]
        public void PasswordHash_PasswordValidationFails_WhenDifferentPasswordUsedForValidationOnHash()
        {
            string originalPassword = "mypassword";
            byte[] salt = _cryptoBLL.GeneratePasswordSalt();
            byte[] passwordHash = _cryptoBLL.PasswordHash(originalPassword, salt);
            string wrongPassword = "wrongPassword";
            Assert.False(_cryptoBLL.VerifyPasswordHash(wrongPassword, passwordHash, salt));
        }

        [Fact]
        public void PasswordHash_PasswordValidationFails_WhenDifferentSaltUsedForValidationOnHash()
        {
            string originalPassword = "mypassword";
            byte[] salt = _cryptoBLL.GeneratePasswordSalt();
            byte[] passwordHash = _cryptoBLL.PasswordHash(originalPassword, salt);
            byte[] wrongSalt = _cryptoBLL.GeneratePasswordSalt();
            Assert.False(_cryptoBLL.VerifyPasswordHash(originalPassword, passwordHash, wrongSalt));
        }
    }
}
