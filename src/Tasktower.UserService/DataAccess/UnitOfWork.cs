﻿using System;
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

        public IUserRepository UserRepo { get; private set; }

        public ICache<T> NewCache<T>(CacheTag tag)
        {
            return new Cache<T>(_cacheDB, tag);
        }

        public UnitOfWork(EntityFrameworkDBContext dbContext, 
            StackExchange.Redis.IDatabase cacheDB)
        {
            _efDbContext = dbContext;
            _cacheDB = cacheDB;
            if (_efDbContext != null) {
                UserRepo = new UserRepository(_efDbContext.UserItems);
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
