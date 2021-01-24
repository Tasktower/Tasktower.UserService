using NHibernate;
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

        private void CloseSession()
        {
            _session.Close();
            _session.Dispose();
            _session = null;
        }

        public void Add(TDomain item)
        {
            _session.Save(item);
        }

        public void AddRange(IEnumerable<TDomain> items)
        {
            foreach(TDomain item in items)
            {
                _session.Save(item);
            }
        }

        public IEnumerable<TDomain> GetAll()
        {
            return _session.Query<TDomain>().ToList();
        }

        public TDomain GetById(object Id)
        {
            return _session.Load<TDomain>(Id);
        }

        public void Remove(TDomain item)
        {
            _session.Delete(item);
        }

        public void RemoveById(object id)
        {
            var item = _session.Load<TDomain>(id);
            _session.Delete(item);
        }

        public void RemoveRange(IEnumerable<TDomain> items)
        {
            foreach (TDomain item in items)
            {
                _session.Delete(item);
            }
        }

        public void Dispose()
        {
            if (_session != null)
            {
                CloseSession();
            }
        }
    }
}
