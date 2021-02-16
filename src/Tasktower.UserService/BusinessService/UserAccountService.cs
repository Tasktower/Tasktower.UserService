using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Dtos;
using Tasktower.Webtools.DependencyInjection;
using Tasktower.UserService.Domain;
using Microsoft.Extensions.Logging;
using Tasktower.Webtools.Security;
using Tasktower.UserService.Errors;
using Mapster;
using Tasktower.UserService.Domain.CacheOnly;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Tasktower.UserService.BusinessService
{
    [ScopedService]
    public class UserAccountService : IUserAccountService
    {
        private readonly ILogger<UserAccountService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public UserAccountService(
            IUnitOfWork unitOfWork, 
            ILogger<UserAccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<UserProfileDto> GetUserByID(Guid id, bool ignoreSensitive = true)
        {
            var user = await _unitOfWork.UserProfileRepo.GetById(id);
            if (user == null)
            {
                throw APIException.Create(APIException.Code.ACCOUNT_NOT_FOUND);
            }
            return UserProfileDto.FromUser(user, ignoreSensitive);
        }
    }
}
