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

        ICache<string> PwdResetTokenCache => NewCache<string>(CacheTag.PASSWORD_RESET_TOKEN);
        ICache<string> RefreshTokenCache => NewCache<string>(CacheTag.REFRESH_TOKEN);
        ICache<string> EmailVerifyTokenCache => NewCache<string>(CacheTag.EMAIL_VERIFY_TOKEN);
        ICache<Domain.User> UserCache => NewCache<Domain.User>(CacheTag.USER);
        ICache<T> NewCache<T>(CacheTag tag);

        void Complete();
        Task CompleteAsync();

        void Rollback();

        Task RollbackAsync();
    }
}
