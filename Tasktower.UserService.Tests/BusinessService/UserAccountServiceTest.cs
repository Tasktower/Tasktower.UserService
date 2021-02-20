using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.Webtools.Security;
using Tasktower.UserService.BusinessService;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Domain;
using Tasktower.UserService.Dtos;
using Tasktower.UserService.Errors;
using Tasktower.UserService.Tests.SetupUtils;
using Xunit;

namespace Tasktower.UserService.Tests.BusinessService
{
    public class UserAccountServiceTest : IDisposable
    {
        private IUnitOfWork _unitOfWork;
        private IUserAccountService _userService;

        public UserAccountServiceTest()
        {
            _unitOfWork = new MockUnitOfWork(GetType().FullName ?? GetType().Name);
            var logger = new NullLogger<UserAccountService>();
            _userService = new UserAccountService(_unitOfWork, logger);
            
            InsertTestData().Wait();
        }

        private async Task InsertTestData()
        {
            var t1 = _unitOfWork.UserProfileRepo.Add(new UserProfile
            {
                Id = Guid.Parse("de0b60db-bd92-4b79-b8fe-ab89837068b2"),
                Name = "Gordon Ramswey",
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            });
            var t2 = _unitOfWork.UserProfileRepo.Add(new UserService.Domain.UserProfile
            {
                Id = Guid.Parse("8e96c807-a3a1-4162-9177-fffd9a60d0da "),
                Name = "Guy Fieri",
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now
            });
            await Task.WhenAll(t1, t2);
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            _unitOfWork.UserProfileRepo.RemoveRange(_unitOfWork.UserProfileRepo.GetAll().Result).Wait();
            _unitOfWork.SaveChanges();
            _unitOfWork.Dispose();
            _unitOfWork = null;
            _userService = null;
        }

        [Fact]
        public async Task GetUserByID_GordonRamsayQueriedWithSensitiveInfo_WhenGordonRamsayIDPassedAndIgnoreSensitiveFalse()
        {
            UserProfile gordonRamsayUser = await _unitOfWork.UserProfileRepo.GetById(Guid.Parse("de0b60db-bd92-4b79-b8fe-ab89837068b2"));
            UserProfileDto userReadDto = await _userService.GetUserByID(gordonRamsayUser.Id, ignoreSensitive: false);
            Assert.Equal(gordonRamsayUser.Id, userReadDto.Id);
            Assert.Equal(gordonRamsayUser.CreatedAt, userReadDto.CreatedAt);
            Assert.Equal(gordonRamsayUser.UpdatedAt, userReadDto.UpdatedAt);
        }

        [Fact]
        public async Task GetUserByID_GordonRamsayQueriedWithSensitiveInfo_WhenGordonRamsayIDPassedAndIgnoreSensitiveTrue()
        {
            UserProfile gordonRamsayUser = await _unitOfWork.UserProfileRepo.GetById(Guid.Parse("de0b60db-bd92-4b79-b8fe-ab89837068b2"));
            UserProfileDto userReadDto = await _userService.GetUserByID(gordonRamsayUser.Id, ignoreSensitive: true);
            Assert.Equal(gordonRamsayUser.Id, userReadDto.Id);
            Assert.Null(userReadDto.CreatedAt);
            Assert.Null(userReadDto.UpdatedAt);
        }

        [Fact]
        public async Task GetUserByID_Fail_WhenWrongIDPassed()
        {
            await _unitOfWork.UserProfileRepo.RemoveRange(_unitOfWork.UserProfileRepo.GetAll().Result);

            var exception = await Assert.ThrowsAsync<APIException>(async () => 
            { 
                await _userService.GetUserByID(Guid.Parse("3777b2ec-f40c-4905-831d-5b4c49be9c28")); 
            });

            Assert.Equal(APIException.Code.ACCOUNT_NOT_FOUND.ToString(), exception.ErrorCode);
        }
    }
}
