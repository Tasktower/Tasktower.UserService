using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.DataAccess.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DbSet<User> context) : base(context) { } 

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _dbContext.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _dbContext.Where(u => u.Email == email).FirstAsync();
        }

        public async Task UpdatePasswordSaltAndPasswordHashByID(Guid id, byte[] passwordHash, byte[] passwordSalt)
        {
            var task = Task.Delay(1);
            var user = await _dbContext.FindAsync(id);
            if (user == null)
            {
                return;
            }
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _dbContext.Update(user);
            await task;
        }

        public async Task UpdateRolesById(Guid id, string[] roles)
        {
            var task = Task.Delay(1);
            var user = await _dbContext.FindAsync(id);
            if (user == null)
            {
                return;
            }
            user.RolesList = roles;
            _dbContext.Update(user);
            await task;
        }

        public async Task UpdateUserDataById(Guid id, string name)
        {
            var task = Task.Delay(1);
            var user = await _dbContext.FindAsync(id);
            if (user == null)
            {
                return;
            }
            user.Name = name;
            _dbContext.Update(user);
            await task;
        }
    }
}
