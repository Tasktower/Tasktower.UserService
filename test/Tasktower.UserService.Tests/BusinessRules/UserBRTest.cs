using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Tests.SetupUtils;
using Xunit;
using Tasktower.UserService.Errors;
using Tasktower.UserService.BusinessService.BusinessRules;

namespace Tasktower.UserService.Tests.BusinessRules
{
    public class UserBRTest : IDisposable
    {
        private IUnitOfWork _unitOfWork;
        private IUserRegisterBR _userRegisterBR;
        public UserBRTest()
        {
            _unitOfWork = new MockUnitOfWork(GetType().FullName ?? GetType().Name);
            _userRegisterBR = new UserRegisterBR(_unitOfWork);
            InsertTestData().Wait();
        }

        private async Task InsertTestData()
        {
            var t1 = _unitOfWork.UserRepo.Add(new UserService.Domain.User
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
            _userRegisterBR = null;
        }

        [Fact]
        public async Task Validate_multipleExceptionsEmailNotFound_whenGordonRamseyMakesExistingAccount()
        {
            var apiEx = await Assert.ThrowsAsync<APIException>(async () => { 
                await _userRegisterBR.Validate(new Dtos.UserRegisterDto { 
                    Name = "Gordon Ramsey",
                    Password = "seasoning",
                    Email = "gordon@email.com"
                }); 
            });
            Assert.Equal(APIException.Code.MULTIPLE_EXCEPTIONS_FOUND.ToString(), apiEx.ErrorCode);
            Assert.Single(apiEx.MultipleErrors);
            Assert.Contains(apiEx.MultipleErrors, e => e.ErrorCode == APIException.Code.ACCOUNT_ALREADY_EXISTS.ToString());
        }

        [Fact]
        public async Task Validate_valid_whenKeauReavesMakesAccount()
        {
            await _userRegisterBR.Validate(new Dtos.UserRegisterDto
                {
                    Name = "Keanu Reaves",
                    Password = "JohnWick",
                    Email = "keanu@email.com"
                });
        }
    }
}
