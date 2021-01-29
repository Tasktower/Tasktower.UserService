using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DataStoreAccessors;

namespace Tasktower.UserService.DataAccess.Repository
{

    public class RepositoryBase<TDomain, IDType> : IRepository<TDomain, IDType> where TDomain : class
    {
        protected DbSet<TDomain> _dbContext;

        public RepositoryBase(DbSet<TDomain> dbContext) {
            _dbContext = dbContext;
        }

        public async Task Add(TDomain item)
        {
            await _dbContext.AddAsync(item);
        }

        public async Task AddRange(IEnumerable<TDomain> items)
        {
            await _dbContext.AddRangeAsync(items.ToArray());
        }

        public async Task<List<TDomain>> GetAll()
        {
            return await _dbContext.ToListAsync();
        }

        public async Task<TDomain> GetById(IDType Id)
        {
            return await _dbContext.FindAsync(Id);
        }

        public async Task Remove(TDomain item)
        {
            var task = Task.Delay(1);
            _dbContext.Remove(item);
            await task;
        }

        public  async Task RemoveById(IDType id)
        {
            var task = Task.Delay(1);
            var item = await _dbContext.FindAsync(id);
            if (item != null) 
            {
                _dbContext.Remove(item);
            }
            await task;
        }

        public async Task RemoveRange(IEnumerable<TDomain> items)
        {
            var task = Task.Delay(1);
            foreach (TDomain item in items)
            {
                _dbContext.Remove(item);
            }
            await task;
        }
    }
}
