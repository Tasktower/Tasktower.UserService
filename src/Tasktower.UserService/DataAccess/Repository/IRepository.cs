using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Repository
{
    public interface IRepository<TDomain>: IDisposable where TDomain : class
    {
        TDomain GetById(object Id);
        IEnumerable<TDomain> GetAll();
        void Add(TDomain obj);
        void AddRange(IEnumerable<TDomain> items);
        void RemoveById(object id);
        void Remove(TDomain item);
        void RemoveRange(IEnumerable<TDomain> items);
        

    }
}
