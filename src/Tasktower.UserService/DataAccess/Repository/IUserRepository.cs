using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.DataAccess.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task UpdateUserDataById(Guid id, string name);
        Task UpdateRolesById(Guid id, IEnumerable<string> roles);
        Task UpdatePasswordSaltAndPasswordHashByID(Guid id, byte[] passwordHash, byte[] passwordSalt);
        Task<User> GetByEmail(string email);

        Task<bool> ExistsByEmail(string email);
    }
}
