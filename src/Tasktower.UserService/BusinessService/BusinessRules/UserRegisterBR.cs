using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Dtos;
using Tasktower.Webutils.DependencyInjection;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Errors;

namespace Tasktower.UserService.BusinessService.BusinessRules
{
    [ScopedService]
    public class UserRegisterBR : IUserRegisterBR
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRegisterBR(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Validate(UserRegisterDto userRegisterDto)
        {
            ICollection<APIException> exceptions = new List<APIException>();
            var userRepo = _unitOfWork.UserRepo;
            var checkEmailExisteTask = userRepo.ExistsByEmail(userRegisterDto.Email);
            if (await checkEmailExisteTask)
            {
                exceptions.Add(APIException.Create(APIException.Code.ACCOUNT_ALREADY_EXISTS));
            }
            if (exceptions.Any())
            {
                throw APIException.CreateFromMultiple(exceptions);
            }

        }
    }
}
