using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.Cache;
using Tasktower.UserService.DataAccess.Repository;
using Tasktower.UserService.Domain.CacheOnly;

namespace Tasktower.UserService.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepo { get; }

        ICache<RefreshTokenData> RefreshTokenHashLocalCache => NewCache<RefreshTokenData>(CacheTag.REFRESH_TOKEN_HASH);
        /// <summary>
        /// Caches RSA public key for jwt verification
        /// </summary>
        ICache<string> AuthRSAPemPubKeyLocalCache => NewCache<string>(CacheTag.AUTH_RSAPEM_PUBKEY);
        ICache<string> AuthRSAPemPubKeySharedCache => NewCache<string>(CacheTag.AUTH_RSAPEM_PUBKEY, shared: true);
        ICache<Domain.User> UserLocalCache => NewCache<Domain.User>(CacheTag.USER);
        /// <summary>
        /// This creates a new cache with a given tag and type. 
        /// This is only meant to and so should not be called by any caller
        /// of this unit of work interface unless the user chooses to add
        /// more cache methods onto the interface.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <param name="shared">If false (by default), local cache is used, otherwise shared cache is used</param>
        /// <returns></returns>
        ICache<T> NewCache<T>(CacheTag tag, bool shared=false);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
