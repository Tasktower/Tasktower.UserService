using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DataStoreAccessors;
using Tasktower.UserService.DataAccess.Repository;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private EFDBContext _efDbContext;

        public IUserRepository UserRepository { get; private set; }
        public UnitOfWork(EFDBContext dbContext)
        {
            _efDbContext = dbContext; 
            if (_efDbContext != null) {
                UserRepository = new UserRepository(_efDbContext.UserItems);
            }
        }

        public void Complete()
        {
            if (_efDbContext != null)
            {
                _efDbContext.SaveChanges();
            }
        }

        public async Task CompleteAsync()
        {
            if (_efDbContext != null)
            {
                await _efDbContext.SaveChangesAsync();
            }
        }
        public async Task RollbackAsync()
        {
            if (_efDbContext != null )
            {
               await _efDbContext.DisposeAsync();
                _efDbContext = null;
            }
        }

        public void Rollback()
        {
            if (_efDbContext != null)
            {
                _efDbContext.Dispose();
                _efDbContext = null;
            }
        }


        public void Dispose()
        {
            if (_efDbContext != null)
            {
                _efDbContext.Dispose();
                _efDbContext = null;

            }
        }
    }
}
