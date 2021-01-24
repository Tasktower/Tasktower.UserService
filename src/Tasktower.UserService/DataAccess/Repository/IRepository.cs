using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Repository
{
    public interface IRepository<TDomain> where TDomain : class
    {
        Task<TDomain> GetById(object Id);
        Task<List<TDomain>> GetAll();
        Task Add(TDomain obj);
        Task AddRange(IEnumerable<TDomain> items);
        Task RemoveById(object id);
        Task Remove(TDomain item);
        Task RemoveRange(IEnumerable<TDomain> items);
        

    }
}
