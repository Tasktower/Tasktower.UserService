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

        public UserServiceTest()
        {
            _unitOfWork = new MockUnitOfWork(GetType().FullName ?? GetType().Name);
            var logger = new NullLogger<UserAccountService>();
            var userRegisterBR = new UserRegisterBR(_unitOfWork);
            _userService = new UserAccountService(_unitOfWork, userRegisterBR, logger);
            
            InsertTestData().Wait();
        }

        private async Task InsertTestData()
        {
            var t1 = _unitOfWork.UserRepo.Add(new User
            {
                Id = Guid.Parse("de0b60db-bd92-4b79-b8fe-ab89837068b2"),
                Name = "Gordon Ramswey",
                Email = "gordon@email.com",
                EmailVerified = true,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            });
            var t2 = _unitOfWork.UserRepo.Add(new UserService.Domain.User
            {
                Id = Guid.Parse("8e96c807-a3a1-4162-9177-fffd9a60d0da "),
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
            Assert.True(CryptoUtils.VerifyPassword(dto.Password, createdUser.PasswordHash, createdUser.PasswordSalt));
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

        [Fact]
        public async Task GetUserByID_GordonRamsayQueriedWithSensitiveInfo_WhenGordonRamsayIDPassedAndIgnoreSensitiveFalse()
        {
            User gordonRamsayUser = await _unitOfWork.UserRepo.GetById(Guid.Parse("de0b60db-bd92-4b79-b8fe-ab89837068b2"));
            UserReadDto userReadDto = await _userService.GetUserByID(gordonRamsayUser.Id, ignoreSensitive: false);
            Assert.Equal(gordonRamsayUser.Id, userReadDto.Id);
            Assert.Equal(gordonRamsayUser.Email, userReadDto.Email);
            Assert.Equal(gordonRamsayUser.EmailVerified, userReadDto.EmailVerified);
            Assert.Equal(gordonRamsayUser.Roles, userReadDto.Roles);
            Assert.Equal(gordonRamsayUser.CreatedAt, userReadDto.CreatedAt);
            Assert.Equal(gordonRamsayUser.UpdatedAt, userReadDto.UpdatedAt);
        }

        [Fact]
        public async Task GetUserByID_GordonRamsayQueriedWithSensitiveInfo_WhenGordonRamsayIDPassedAndIgnoreSensitiveTrue()
        {
            User gordonRamsayUser = await _unitOfWork.UserRepo.GetById(Guid.Parse("de0b60db-bd92-4b79-b8fe-ab89837068b2"));
            UserReadDto userReadDto = await _userService.GetUserByID(gordonRamsayUser.Id, ignoreSensitive: true);
            Assert.Equal(gordonRamsayUser.Id, userReadDto.Id);
            Assert.Equal(gordonRamsayUser.Email, userReadDto.Email);
            Assert.Null(userReadDto.EmailVerified);
            Assert.Null( userReadDto.Roles);
            Assert.Null(userReadDto.CreatedAt);
            Assert.Null(userReadDto.UpdatedAt);
        }

        [Fact]
        public async Task GetUserByID_Fail_WhenWrongIDPassed()
        {
            await _unitOfWork.UserRepo.RemoveRange(_unitOfWork.UserRepo.GetAll().Result);

            var exception = await Assert.ThrowsAsync<APIException>(async () => 
            { 
                await _userService.GetUserByID(Guid.Parse("3777b2ec-f40c-4905-831d-5b4c49be9c28")); 
            });

            Assert.Equal(APIException.Code.ACCOUNT_NOT_FOUND, exception.ErrorCode);
        }
    }
}
