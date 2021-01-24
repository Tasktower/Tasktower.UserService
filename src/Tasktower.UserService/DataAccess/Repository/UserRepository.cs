using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.DataAccess.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ISession session) : base(session) { } 

        public bool ExistsByEmail(string email)
        {
            return _session.Query<User>().Any(u => u.Email == email);
        }

        public User? GetByEmail(string email)
        {
            return _session.Query<User>().Where(u => u.Email == email).FirstOrDefault();
        }

        public void UpdatePasswordSaltAndPasswordHashByID(Guid id, byte[] passwordHash, byte[] passwordSalt)
        {
            _session.CreateQuery(@" 
                UPDATE u FROM User as u 
                Set u.PasswordHash = :PasswordHash, u.PasswordSalt = :PasswordSalt  
                WHERE u.Id = :Id")
                .SetParameter(":Id", id)
                .SetParameter(":PasswordHash", passwordHash)
                .SetParameter(":PasswordSalt", passwordSalt)
                .ExecuteUpdate();
        }

        public void UpdateRolesById(Guid id, IEnumerable<string> roles)
        {
            _session.CreateQuery(@" 
                UPDATE u FROM User as u 
                Set u.Roles = :Roles  
                WHERE u.Id = :Id")
                .SetParameter(":Id", id)
                .SetParameter(":Roles", roles)
                .ExecuteUpdate();
        }

        public void UpdateUserDataById(Guid id, string name)
        {
            _session.CreateQuery(@" 
                UPDATE u FROM User as u 
                Set u.Name = :Name  
                WHERE u.Id = :Id")
                .SetParameter(":Id", id)
                .SetParameter(":Name", name)
                .ExecuteUpdate();
        }
    }
}
