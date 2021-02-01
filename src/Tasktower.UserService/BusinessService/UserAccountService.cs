using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Dtos;
using Tasktower.UserService.Utils.DependencyInjection;
using Tasktower.UserService.BusinessRules;
using Tasktower.UserService.Domain;
using Microsoft.Extensions.Logging;
using Tasktower.UserService.Errors;
using Mapster;

namespace Tasktower.UserService.BusinessService
{
    [BusinessService]
    public class UserAccountService : IUserAccountService
    {
        private readonly ILogger<UserAccountService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRegisterBR _userRegisterBR;
        private readonly ICryptoService _cryptoService;
        public UserAccountService(
            IUnitOfWork unitOfWork, 
            IUserRegisterBR userRegisterBR,
            ICryptoService cryptoService,
            ILogger<UserAccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _userRegisterBR = userRegisterBR;
            _cryptoService = cryptoService;
            _logger = logger;
        }

        public async Task<UserReadDto> GetUserByID(Guid id)
        {
            var user = await _unitOfWork.UserRepo.GetById(id);
            if (user == null)
            {
                throw APIException.Create(APIException.Code.ACCOUNT_NOT_FOUND);
            }
            return user.Adapt<UserReadDto>();
        }

        public async Task<UserReadDto> RegisterUser(UserRegisterDto userRegisterDto, bool emailVerified=false)
        {
            await _userRegisterBR.Validate(userRegisterDto);
            var passwordSalt = _cryptoService.GeneratePasswordSalt();
            var passwordHash = _cryptoService.PasswordHash(userRegisterDto.Password, passwordSalt);
            var user = new User
            {
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                EmailVerified = emailVerified,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            await _unitOfWork.UserRepo.Add(user);
            await _unitOfWork.SaveChangesAsync();

            if (!emailVerified)
            {
                _logger.LogInformation("Sending email verify link");
            }
            return user.Adapt<UserReadDto>();
        }
    }
}
