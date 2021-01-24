using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Domain
{
    public class User : AbstractDomain
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public bool EmailVerified { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }

        public class UserMap : User.AbstractDomainMap<User> {
            public UserMap() : base() {
                Table("users");
                Map(x => x.Name).Column("name");
                Map(x => x.Email).Column("email");
                Map(x => x.EmailVerified).Column("email_verified");
                Map(x => x.Roles).Column("roles");
                Map(x => x.PasswordSalt).Column("password_salt");
                Map(x => x.PasswordHash).Column("password_hash");
            }
        }
    }
}
