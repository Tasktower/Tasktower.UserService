using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Cache
{
    public class PwdResetTokenCache : Cache<string>, IPwdResetTokenCache
    {
        public PwdResetTokenCache(IDatabase cacheDB) : base(cacheDB, "PWD_RESET_TOKEN")
        {

        }
    }
}
