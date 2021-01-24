using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Domain
{
    [Table("users")]
    public class User : AbstractDomain
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }

        [Column("email_verified")]
        public bool EmailVerified { get; set; }

        [Column("roles")]
        public string Roles { get; set; }
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; }
        [Column("password_hash")]
        public byte[] PasswordHash { get; set; }

        public static string RolesListToRoles(string[] rolesList)
        {
            return string.Join(",", rolesList);
        }

        public static string[] RolesToRolesList(string roles)
        {
            return roles.Split(",");
        }   
    }
}
