using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DataStoreAccessors;
using Tasktower.UserService.DataAccess.Cache;
using Tasktower.UserService.DataAccess.Repository;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private EntityFrameworkDBContext _efDbContext;
        private StackExchange.Redis.IDatabase _cacheDB;
        private StackExchange.Redis.IDatabase _keyvaultDB;

        public IUserRepository UserRepo { get; private set; }
        public IPwdResetTokenCache PwdResetTknCache { get; private set; }
        public IRefreshTokenCache RefreshTknCache { get; private set; }
        public UnitOfWork(EntityFrameworkDBContext dbContext, 
            StackExchange.Redis.IDatabase cacheDB,
            StackExchange.Redis.IDatabase keyvaultDB)
        {
            _efDbContext = dbContext;
            _cacheDB = cacheDB;
            _keyvaultDB = keyvaultDB;
            if (_efDbContext != null) {
                UserRepo = new UserRepository(_efDbContext.UserItems);
            }
            if (_cacheDB != null)
            {
                PwdResetTknCache = new PwdResetTokenCache(_cacheDB);
                RefreshTknCache = new RefreshTokenCache(_cacheDB);
            }
            if (_keyvaultDB != null)
            {
                // init keyvault
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
