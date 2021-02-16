using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.DataAccess.Repository
{
    public interface IUserProfileRepository : IRepository<UserProfile, Guid>
    {
    }
}
