using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.Repository;

namespace Tasktower.UserService.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get;  }

        void Complete();
        Task CompleteAsync();

        void Rollback();

        Task RollbackAsync();
    }
}
