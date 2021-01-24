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

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _session.Query<User>().AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _session.Query<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task UpdatePasswordSaltAndPasswordHashByID(Guid id, byte[] passwordHash, byte[] passwordSalt)
        {
           await  _session.CreateQuery(@" 
                UPDATE u FROM User as u 
                Set u.PasswordHash = :PasswordHash, u.PasswordSalt = :PasswordSalt  
                WHERE u.Id = :Id")
                .SetParameter(":Id", id)
                .SetParameter(":PasswordHash", passwordHash)
                .SetParameter(":PasswordSalt", passwordSalt)
                .ExecuteUpdateAsync();
        }

        public async Task UpdateRolesById(Guid id, string[] roles)
        {
            await _session.CreateQuery(@" 
                UPDATE u FROM User as u 
                Set u.Roles = :Roles  
                WHERE u.Id = :Id")
                .SetParameter(":Id", id)
                .SetParameter(":Roles", roles)
                .ExecuteUpdateAsync();
        }

        public async Task UpdateUserDataById(Guid id, string name)
        {
            await _session.CreateQuery(@" 
                UPDATE u FROM User as u 
                Set u.Name = :Name  
                WHERE u.Id = :Id")
                .SetParameter(":Id", id)
                .SetParameter(":Name", name)
                .ExecuteUpdateAsync();
        }
    }
}
