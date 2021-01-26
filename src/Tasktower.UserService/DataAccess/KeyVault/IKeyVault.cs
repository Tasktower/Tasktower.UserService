using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.KeyVault
{
    public interface IKeyVault<T>
    {
        Task<T> Get(string id);
        Task SetIfNotExists(string id, T value, TimeSpan? absoluteExpireTime = null);
        Task Set(string id, T value, TimeSpan? absoluteExpireTime = null);
    }
}
