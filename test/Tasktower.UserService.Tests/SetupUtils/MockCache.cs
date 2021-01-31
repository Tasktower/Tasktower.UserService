using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.Cache;

namespace Tasktower.UserService.Tests.SetupUtils
{
    class MockCache<T> : ICache<T>
    {
        private static readonly string _storageName = "User_Service";

        private readonly Dictionary<string, (string, DateTime)> _store;
        protected readonly string _prefix;

        protected string FullKey(string id)
        {
            return $"{_prefix}:{id}";
        }

        public MockCache(Dictionary<string, (string, DateTime)> store, CacheTag tag) 
        {
            Type type = typeof(T);
            _prefix = $"{_storageName}:{type.FullName ?? type.Name}:{tag}";
            _store = store;
        }

        public async Task Delete(string id)
        {
            var task = Task.Delay(1);
            _store.Remove(FullKey(id));
            await task;
        }

        public async Task<T> Get(string id)
        {
            var task = Task.Delay(1);
            (string, DateTime) data = _store.GetValueOrDefault(FullKey(id));
            await task;
            if (data.Item1 is null || data.Item2 < DateTime.Now)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data.Item1);
        }

        public async Task Set(string id, T value, TimeSpan? absoluteExpireTime = null)
        {
            var task = Task.Delay(1);
            var date = DateTime.Now.Add(absoluteExpireTime ?? DateTime.MaxValue.Subtract(DateTime.Now));
            _store.Add(FullKey(id), (JsonSerializer.Serialize(value), date));
            await task;
        }

        public async Task SetIfNotExists(string id, T value, TimeSpan? absoluteExpireTime = null)
        {
            var task = Task.Delay(1);
            if (!_store.ContainsKey(FullKey(id)) || _store.GetValueOrDefault(FullKey(id)).Item2 < DateTime.Now)
            {
                var date = DateTime.Now.Add(absoluteExpireTime ?? DateTime.MaxValue.Subtract(DateTime.Now));
                _store.Add(FullKey(id), (JsonSerializer.Serialize(value), date));
            }

            await task;
        }
    }
}
