using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;
using Tasktower.UserService.Dtos;

namespace Tasktower.UserService.BusinessService
{
    public interface IUserAccountService
    {
        Task<UserReadDto> RegisterUser(UserRegisterDto userRegisterDto, bool emailVerified = false);
        Task<UserReadDto> GetUserByID(Guid id);
    }
}
