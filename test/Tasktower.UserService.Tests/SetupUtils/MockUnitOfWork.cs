﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess;
using Tasktower.UserService.DataAccess.Cache;
using Tasktower.UserService.DataAccess.DBAccessor;
using Tasktower.UserService.DataAccess.Repository;

namespace Tasktower.UserService.Tests.SetupUtils
{
    class MockUnitOfWork : IUnitOfWork
    {
        private Dictionary<string, (string, DateTime)> _cacheStore;
        private Dictionary<string, (string, DateTime)> _sharedCacheStore;
        private EntityFrameworkDBContext _efDbContext;

        public IUserProfileRepository UserProfileRepo { get; private set; }

        public ICache<T> NewLocalCache<T>(CacheTag tag)
        {
            return new MockCache<T>(_cacheStore, tag);
        }

        public ICache<T> NewCache<T>(CacheTag tag, bool shared = false)
        {
            return new MockCache<T>(_sharedCacheStore, tag);
        }

        public MockUnitOfWork(string dbname)
        {
            _cacheStore = new Dictionary<string, (string, DateTime)>();
            _sharedCacheStore = new Dictionary<string, (string, DateTime)>();
            var optionsBuilder = new DbContextOptionsBuilder<EntityFrameworkDBContext>();
            optionsBuilder.UseInMemoryDatabase(dbname);
            _efDbContext = new EntityFrameworkDBContext(optionsBuilder.Options);
            UserProfileRepo = new UserProfileRepository(_efDbContext.UserItems);
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
                _efDbContext.Dispose();
                _efDbContext = null;
                _cacheStore = null;

            }
        }

    }
}
