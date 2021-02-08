using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.Webutils.Security.Auth;
using Tasktower.UserService.Domain;
using Xunit;

namespace Tasktower.UserService.Tests.Domain
{
    public class UserTest
    {

        [Fact]
        public void CreateUser_CorrectUserCreated_WithGeneratedFields() {
            // ARRANGE
            var now = DateTime.UtcNow;
            var id = Guid.NewGuid();
            var randomGenerator = new Random();
            var passwordHash = new byte[32];
            randomGenerator.NextBytes(passwordHash);
            var passwordSalt = new byte[8];
            randomGenerator.NextBytes(passwordSalt);
            // ACT
            var user = new User
            {
                Id = id,
                CreatedAt = now,
                UpdatedAt = now,
                Name = "Super User",
                Email = "superuser@example.com",
                EmailVerified = true,
                Roles = new Role[]{ Role.SUPERUSER },
                PasswordSalt = passwordSalt,
                PasswordHash = passwordHash
            };

            // ASSERT
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               Assert.Equal(id, user.Id);
            Assert.Equal(now, user.CreatedAt);
            Assert.Equal(now, user.UpdatedAt);
            Assert.Equal("Super User", user.Name);
            Assert.Equal("superuser@example.com", user.Email);
            Assert.True(user.EmailVerified);
            Assert.Equal(new Role[] { Role.SUPERUSER }, user.Roles);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.Equal(passwordSalt, user.PasswordSalt);
        }
    }
}
