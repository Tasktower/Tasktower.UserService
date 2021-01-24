using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.DataAccess.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        void UpdateUserDataById(Guid id, string name);
        void UpdateRolesById(Guid id, IEnumerable<string> roles);
        void UpdatePasswordSaltAndPasswordHashByID(Guid id, byte[] passwordHash, byte[] passwordSalt);
        User GetByEmail(string email);

        bool ExistsByEmail(string email);
    }
}
