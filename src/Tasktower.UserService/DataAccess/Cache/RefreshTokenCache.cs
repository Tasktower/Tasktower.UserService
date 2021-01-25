using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Cache
{
    public class RefreshTokenCache : Cache<string>, IRefreshTokenCache
    {
        public RefreshTokenCache(IDatabase cacheDB) : base(cacheDB, "REFRESH_TOKEN")
        {
        }
    }
}
