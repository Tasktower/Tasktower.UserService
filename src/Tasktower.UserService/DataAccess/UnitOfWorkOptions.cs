using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.DataAccess.DBAccessor;

namespace Tasktower.UserService.DataAccess
{
    public class UnitOfWorkOptions
    {
        public DbContextOptions<EntityFrameworkDBContext> DBContextOptions { get; set; }
        public string LocalCacheConnectionString { get; set; }
        public string SharedCacheConnectionString { get; set; }

    }
}
