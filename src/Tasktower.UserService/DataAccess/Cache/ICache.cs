using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Cache
{
    public interface ICache<T>
    {
        Task<T> Get(string id);
        Task SetIfNotExists(string id, T value, TimeSpan? absoluteExpireTime = null);
        Task Set(string id, T value, TimeSpan? absoluteExpireTime = null);
        Task Delete(string id);

    }
}
