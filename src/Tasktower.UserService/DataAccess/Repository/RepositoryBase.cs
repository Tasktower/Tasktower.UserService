using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Repository
{

    public class RepositoryBase<TDomain> : IRepository<TDomain> where TDomain : class
    {
        protected ISession _session = null;

        public RepositoryBase(ISession session) {
            _session = session;
        }

        public async Task Add(TDomain item)
        {
           await _session.SaveAsync(item);
        }

        public async Task AddRange(IEnumerable<TDomain> items)
        {
            IList<Task> tasks = new List<Task>(items.Count() + 1);
            foreach(TDomain item in items)
            {
                tasks.Append(_session.Save(item));
            }
            await Task.WhenAll(tasks);
        }

        public async Task<List<TDomain>> GetAll()
        {
            return await _session.Query<TDomain>().ToListAsync();
        }

        public async Task<TDomain> GetById(object Id)
        {
            return await _session.LoadAsync<TDomain>(Id);
        }

        public async Task Remove(TDomain item)
        {
            await _session.DeleteAsync(item);
        }

        public  async Task RemoveById(object id)
        {
            var item = await _session.LoadAsync<TDomain>(id);
            await _session.DeleteAsync(item);
        }

        public async Task RemoveRange(IEnumerable<TDomain> items)
        {
            IList<Task> tasks = new List<Task>(items.Count() + 1);
            foreach (TDomain item in items)
            {
               tasks.Append(_session.DeleteAsync(item));
            }
            await Task.WhenAll(tasks.ToArray());
        }
    }
}
