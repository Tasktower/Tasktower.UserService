using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DBAccessor;
using Tasktower.UserService.DataAccess.Cache;
using Tasktower.UserService.DataAccess.Repository;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private EntityFrameworkDBContext _efDbContext;
        private StackExchange.Redis.IDatabase _cacheDB;

        public IUserRepository UserRepo { get; private set; }

        public ICache<T> NewCache<T>(CacheTag tag)
        {
            return new Cache<T>(_cacheDB, tag);
        }

        public UnitOfWork(IOptions<UnitOfWorkOptions> options)
        {
            _efDbContext = new EntityFrameworkDBContext(options.Value.DBContextOptions);
            _cacheDB = ConnectionMultiplexer.Connect(options.Value.CacheConnectionString).GetDatabase();
            UserRepo = new UserRepository(_efDbContext.UserItems);
        }

        public void SaveChanges()
        {
            if (_efDbContext != null)
            {
                _efDbContext.SaveChanges();
            }
        }

        public async Task SaveChangesAsync()
        {
            if (_efDbContext != null)
            {
                await _efDbContext.SaveChangesAsync();
            }
        }


        public void Dispose()
        {
            if (_efDbContext != null)
            {
                _efDbContext.DisposeAsync();
                _efDbContext = null;

            }
        }
    }
}
