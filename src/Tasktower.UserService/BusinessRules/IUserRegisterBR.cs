using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Dtos;

namespace Tasktower.UserService.BusinessRules
{
    public interface IUserRegisterBR
    {
        Task Validate(UserRegisterDto userRegisterDto);
    }
}
