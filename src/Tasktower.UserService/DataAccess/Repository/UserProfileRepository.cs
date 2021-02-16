using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.DataAccess.Repository
{
    public class UserProfileRepository : RepositoryBase<UserProfile, Guid>, IUserProfileRepository
    {
        public UserProfileRepository(DbSet<UserProfile> context) : base(context) { } 
    }
}
