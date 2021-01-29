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
            return $"{_prefix}:{id}";
        }

        IDatabase _cacheDB;

        public Cache(IDatabase cacheDB, CacheTag tag)
        {
            Type type = typeof(T);
            var typename = type.FullName ?? type.Name;
            var prefix = $"{_storageName}:{typename}:{tag}";
            _cacheDB = cacheDB;
            _prefix = prefix;
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
