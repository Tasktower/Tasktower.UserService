using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Domain
{
    public class User : AbstractDomain
    {
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }

        public virtual bool EmailVerified { get; set; }
        public virtual IList<string> Roles { get; set; }
        public virtual byte[] PasswordSalt { get; set; }
        public virtual byte[] PasswordHash { get; set; }

        public class UserMap : User.AbstractDomainMap<User> {
            public UserMap() : base() {
                Table("users");
                Map(x => x.Name).Column("name").CustomSqlType("uuid");
                Map(x => x.Email).Column("email");
                Map(x => x.EmailVerified).Column("email_verified");
                Map(x => x.Roles).Column("roles").CustomSqlType("text[]");
                Map(x => x.PasswordSalt).Column("password_salt");
                Map(x => x.PasswordHash).Column("password_hash");
            }
        }
    }
}
