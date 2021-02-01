using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.UserService.BusinessRules;
using Tasktower.UserService.BusinessService;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Domain;
using Tasktower.UserService.Dtos;
using Tasktower.UserService.Errors;
using Tasktower.UserService.Security;
using Tasktower.UserService.Tests.SetupUtils;
using Xunit;

namespace Tasktower.UserService.Tests.BusinessService
{
    public class UserServiceTest : IDisposable
    {
        private IUnitOfWork _unitOfWork;
        private IUserAccountService _userService;
        private ICryptoService _cryptoService;

        public UserServiceTest()
        {
            _unitOfWork = new MockUnitOfWork(GetType().FullName ?? GetType().Name);
            var logger = new NullLogger<UserService.BusinessService.UserAccountService>();
            var userRegisterBR = new UserRegisterBR(_unitOfWork);
            _cryptoService = new CryptoService();
            _userService = new UserAccountService(_unitOfWork, userRegisterBR, _cryptoService, logger);
            
            InsertTestData().Wait();
        }

        private async Task InsertTestData()
        {
            var t1 = _unitOfWork.UserRepo.Add(new User
            {
                Id = Guid.NewGuid(),
                Name = "Gordon Ramswey",
                Email = "gordon@email.com",
                EmailVerified = true,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            });
            var t2 = _unitOfWork.UserRepo.Add(new UserService.Domain.User
            {
                Id = Guid.NewGuid(),
                Name = "Guy Fieri",
                Email = "guy@email.com",
                EmailVerified = true,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            });
            await Task.WhenAll(t1, t2);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.UserRepo.RemoveRange(_unitOfWork.UserRepo.GetAll().Result).Wait();
            _unitOfWork.SaveChanges();
            _unitOfWork.Dispose();
            _unitOfWork = null;
            _cryptoService = null;
            _userService = null;
        }

        [Fact]
        public async Task Registeruser_correctuserregistered_whenKeanuRegisters()
        {
            var dto = new UserRegisterDto 
            {
                Name = "keanu",
                Email = "keanu@gmail.com",
                Password = "password123"
            };
            await _userService.RegisterUser(dto);
            User createdUser = await _unitOfWork.UserRepo.GetByEmail(dto.Email);
            Assert.Equal(dto.Name, createdUser.Name);
            Assert.Equal(dto.Email, createdUser.Email);
            Assert.True(_cryptoService.VerifyPasswordHash(dto.Password, createdUser.PasswordHash, createdUser.PasswordSalt));
            Assert.Equal(new Role[] { Role.STANDARD }, createdUser.Roles);
        }

        [Fact]
        public async Task Registeruser_registrationFails_whenGordonRamsayRegisters()
        {
            var dto = new UserRegisterDto
            {
                Name = "Gordon Ramsey",
                Password = "seasoning",
                Email = "gordon@email.com"
            };

            var apiEx = await Assert.ThrowsAsync<APIException>(async () => 
            {
                await _userService.RegisterUser(dto);
            });

            Assert.Equal(APIException.Code.MULTIPLE_EXCEPTIONS_FOUND, apiEx.ErrorCode);
            Assert.Single(apiEx.MultipleErrors);
            Assert.Contains(apiEx.MultipleErrors, e => e.ErrorCode == APIException.Code.ACCOUNT_ALREADY_EXISTS);
        }
    }
}
