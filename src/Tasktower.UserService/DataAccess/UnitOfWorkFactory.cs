using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DataStoreAccessors;
using Microsoft.EntityFrameworkCore;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConfiguration _configuration;
        private readonly DbContextOptions<EFDBContext> _efdbOptions;
        public UnitOfWorkFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            _efdbOptions = new DbContextOptionsBuilder<EFDBContext>()
                .UseSqlServer(_configuration.GetConnectionString("mssqlconnection"))
                .Options;
        }

        public IUnitOfWork Create(bool useDatabase = true, bool useMemStore = true, bool useKeyVault = true)
        {
            return new UnitOfWork(useDatabase ? new EFDBContext(_efdbOptions) : null);
        }
    }
}
