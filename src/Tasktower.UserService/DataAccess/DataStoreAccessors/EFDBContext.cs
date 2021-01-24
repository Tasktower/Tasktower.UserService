using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.DataAccess.DataStoreAccessors
{
    public class EFDBContext : DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options)
        {
        }
        public DbSet<User> UserItems { get; set; }

    }
}