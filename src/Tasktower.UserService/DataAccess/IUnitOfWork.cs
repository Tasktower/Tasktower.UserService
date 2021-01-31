using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.Cache;
using Tasktower.UserService.DataAccess.Repository;

namespace Tasktower.UserService.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepo { get; }

        ICache<string> PwdResetTokenCache => NewCache<string>(CacheTag.PASSWORD_RESET_TOKEN);
        ICache<string> RefreshTokenCache => NewCache<string>(CacheTag.REFRESH_TOKEN);
        ICache<string> EmailVerifyTokenCache => NewCache<string>(CacheTag.EMAIL_VERIFY_TOKEN);
        /// <summary>
        /// Caches RSA public key for jwt verification
        /// </summary>
        ICache<string> AuthJWTPubRSAPemKeyCache => NewCache<string>(CacheTag.AUTHJWT_PUBLIC_RSA_PEMKEY);
        ICache<Domain.User> UserCache => NewCache<Domain.User>(CacheTag.USER);
        /// <summary>
        /// This creates a new cache with a given tag and type. 
        /// This is only meant to and so should not be called by any caller
        /// of this unit of work interface unless the user chooses to add
        /// more cache methods onto the interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <returns></returns>
        ICache<T> NewCache<T>(CacheTag tag);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
