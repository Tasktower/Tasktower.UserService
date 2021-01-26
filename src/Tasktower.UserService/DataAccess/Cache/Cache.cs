using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tasktower.UserService.DataAccess.Cache
{
    public class Cache<T> : ICache<T>
    {
        private static readonly string _storageName = "User_Service";
        protected readonly string _prefix;

        protected string FullKey(string id) 
        {
            return $"{_storageName}_{_prefix}_{id}";
        }

        IDatabase _cacheDB;

        public Cache(IDatabase cacheDB, string prefix) 
        {
            _cacheDB = cacheDB;
            _prefix = prefix;
        }

        public Cache(IDatabase cacheDB)
        {
            _cacheDB = cacheDB;
            var type = typeof(T);
            _prefix = type.FullName ?? type.Name;
        }

        public async Task<T> Get(string id)
        {
            string jsonData = await _cacheDB.StringGetAsync(FullKey(id));

            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        private async Task SetWithOptions(string id, T value, TimeSpan? absoluteExpireTime = null, When when = When.Always)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _cacheDB.StringSetAsync(FullKey(id), jsonData, 
                absoluteExpireTime ?? TimeSpan.FromSeconds(60), when);
        }

        public async Task SetIfNotExists(string id, T value, TimeSpan? absoluteExpireTime = null)
        {
            await SetWithOptions(id, value, absoluteExpireTime, When.NotExists);
        }

        public async Task Set(string id, T value, TimeSpan? absoluteExpireTime = null)
        {
            await SetWithOptions(id, value, absoluteExpireTime, When.Always);
        }

        public async Task Delete(string id)
        {
            await _cacheDB.KeyDeleteAsync(FullKey(id));
        }
    }
}
