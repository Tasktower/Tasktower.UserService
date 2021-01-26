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
        IUserRepository UserRepo { get;  }

        ICache<string> PwdResetTokenCache => NewCache<string>(CachePrefix.PASSWORD_RESET_TOKEN);
        ICache<string> RefreshTokenCache => NewCache<string>(CachePrefix.REFRESH_TOKEN);
        ICache<string> EmailVerifyTokenCache => NewCache<string>(CachePrefix.EMAIL_VERIFY_TOKEN);

        ICache<T> NewCache<T>(CachePrefix prefix);
        ICache<T> NewCache<T>(string prefix = "")
        {
            Type type = typeof(T);
            var typename = type.FullName ?? type.Name;
            var fullprefix = $"{typename}_{prefix}";

            if (Enum.IsDefined(typeof(CachePrefix), fullprefix))
            {
                throw new ArgumentException("Prefix already used");
            }
            return NewRawCache<T>(fullprefix);
        }

        /// <summary>
        /// Creates a new cache with any prefix name.
        /// This method is only intended to help other methods
        /// and should generally not be used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefix"></param>
        /// <returns></returns>
        ICache<T> NewRawCache<T>(string prefix);

        void Complete();
        Task CompleteAsync();

        void Rollback();

        Task RollbackAsync();
    }
}
