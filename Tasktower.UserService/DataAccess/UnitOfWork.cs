using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DBAccessor;
using Tasktower.UserService.DataAccess.Cache;
using Tasktower.UserService.DataAccess.Repository;
using StackExchange.Redis;
using Microsoft.Extensions.Options;

namespace Tasktower.UserService.DataAccess
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private EntityFrameworkDBContext _efDbContext;
        private IConnectionMultiplexer _localCacheMuxer;
        private IConnectionMultiplexer _sharedCacheMuxer;

        public IUserProfileRepository UserProfileRepo { get; private set; }

        public ICache<T> NewCache<T>(CacheTag tag, bool shared=false)
        {
            var muxer = shared ? _sharedCacheMuxer : _localCacheMuxer;
            return new Cache<T>(muxer?.GetDatabase(), tag);
        }

        public UnitOfWork(IOptions<UnitOfWorkOptions> options)
        {
            _efDbContext = new EntityFrameworkDBContext(options.Value.DBContextOptions);
            _localCacheMuxer = ConnectionMultiplexer.Connect(options.Value.LocalCacheConnectionString);
            _sharedCacheMuxer = ConnectionMultiplexer.Connect(options.Value.SharedCacheConnectionString);
            UserProfileRepo = new UserProfileRepository(_efDbContext.UserItems);
        }

        public void SaveChanges()
        {
            _efDbContext?.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _efDbContext?.SaveChangesAsync();
        }


        public void Dispose()
        {
            _efDbContext?.Dispose();
            _efDbContext = null;
            _localCacheMuxer?.Dispose();
            _localCacheMuxer = null;
            _sharedCacheMuxer?.Dispose();
            _sharedCacheMuxer = null;
        }
    }
}
