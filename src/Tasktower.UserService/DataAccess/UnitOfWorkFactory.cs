using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DataStoreAccessors;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConfiguration _configuration;
        private readonly DbContextOptions<EntityFrameworkDBContext> _efdbOptions;
        private readonly IConnectionMultiplexer _cacheMuxer;
        public UnitOfWorkFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _efdbOptions = new DbContextOptionsBuilder<EntityFrameworkDBContext>()
                .UseSqlServer(_configuration.GetConnectionString("mssqlconnection"))
                .Options;
            _cacheMuxer = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("redisMemStoreConn"));
        }

        public IUnitOfWork Create(bool useDatabase = true, bool useCache = true)
        {
            return new UnitOfWork(
                useDatabase ? new EntityFrameworkDBContext(_efdbOptions) : null,
                useCache? _cacheMuxer.GetDatabase(): null);
        }
    }
}
