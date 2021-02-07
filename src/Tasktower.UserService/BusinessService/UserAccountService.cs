using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.Dtos;
using Tasktower.UserService.Utils.DependencyInjection;
using Tasktower.UserService.Domain;
using Microsoft.Extensions.Logging;
using Tasktower.UserService.Errors;
using Mapster;
using Tasktower.UserService.Security.Auth.AuthData;
using Tasktower.UserService.Domain.CacheOnly;
using Tasktower.UserService.Security;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Tasktower.UserService.BusinessService.BusinessRules;

namespace Tasktower.UserService.BusinessService
{
    [ScopedService]
    public class UserAccountService : IUserAccountService
    {
        private readonly ILogger<UserAccountService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRegisterBR _userRegisterBR;
        public UserAccountService(
            IUnitOfWork unitOfWork, 
            IUserRegisterBR userRegisterBR,
            ILogger<UserAccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _userRegisterBR = userRegisterBR;
            _logger = logger;
        }

        public async Task<UserReadDto> GetUserByID(Guid id, bool ignoreSensitive = true)
        {
            var user = await _unitOfWork.UserRepo.GetById(id);
            if (user == null)
            {
                throw APIException.Create(APIException.Code.ACCOUNT_NOT_FOUND);
            }
            return UserReadDto.FromUser(user, ignoreSensitive);
        }

        public async Task<AuthTokensDto> SignInStandard(UserStandardSignInDto userStandardSignInDto)
        {
            var user = await _unitOfWork.UserRepo.GetByEmail(userStandardSignInDto.Email);
            if (user == null || user.PasswordHash == null || user.PasswordSalt == null || 
                !CryptoUtils.VerifyPassword(userStandardSignInDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw APIException.Create(APIException.Code.ACCOUNT_NOT_FOUND);
            }

            Task<string> createResreshTokenTask = CreateSaveRefreshToken(user);

            string xsrfToken = Convert.ToBase64String(CryptoUtils.GenerateRandomBytes(32));
            Task<string> createAccessTokenTask = CreateAccessToken(user, xsrfToken);

            return new AuthTokensDto
            {
                RefreshToken = await createResreshTokenTask,
                AccessToken = await createAccessTokenTask,
                XSRFToken = xsrfToken
            };
        }

        private async Task<string> CreateSaveRefreshToken(User user)
        {
            byte[] refreshTokenBytes = CryptoUtils.GenerateRandomBytes(64);
            await _unitOfWork.RefreshTokenHashLocalCache.SetIfNotExists(
                Convert.ToBase64String(CryptoUtils.CreateSHA256Hash(refreshTokenBytes)),
                new RefreshTokenData { UserID = user.Id },
                TimeSpan.FromDays(4));
            return Convert.ToBase64String(refreshTokenBytes);
        }

        private async Task<string> CreateAccessToken(User user, string xsrfToken)
        {
            DateTime now = DateTime.Now;
            var kid = Guid.NewGuid().ToString();
            using RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096);
            Task savePubKeySharedTask = _unitOfWork.AuthRSAPemPubKeySharedCache.SetIfNotExists(kid, 
                CryptoUtils.CreateRSAPublicPem(rsa), TimeSpan.FromMinutes(70));

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa) { KeyId = kid}, SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            UserAuthData claims = new UserAuthData
            {
                EmailVerified = user.EmailVerified,
                Roles = user.Roles,
                UserID = user.Id,
                XSRFToken = xsrfToken
            };
            string token = CryptoUtils.CreateJWTToken(signingCredentials, claims.ToJWTClaims(now), now, now.AddHours(1));
            await savePubKeySharedTask;
            
            return token;
            
        }

        public async Task<UserReadDto> RegisterUser(UserRegisterDto userRegisterDto, 
            bool emailVerified=false, bool ignoreSensitive = true)
        {
            await _userRegisterBR.Validate(userRegisterDto);
            var passwordSalt = CryptoUtils.GenerateRandomBytes(size: 32);
            var passwordHash = CryptoUtils.CreatePasswordHash(userRegisterDto.Password, passwordSalt);
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

            var createdUserTask = _unitOfWork.UserRepo.GetByEmail(user.Email);

            if (!emailVerified)
            {
                // TODO: generate and send email token
                _logger.LogInformation("Sending email verify link");
            }
            var createdUser = await createdUserTask;
            return UserReadDto.FromUser(createdUser, ignoreSensitive);
        }
    }
}
