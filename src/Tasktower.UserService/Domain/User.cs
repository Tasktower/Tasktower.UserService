using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tasktower.Webtools.Security.Auth;

namespace Tasktower.UserService.Domain
{
    [Table("users")]
    public class User : AbstractDomain
    {

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(320)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("email_verified")]
        public bool EmailVerified { get; set; }

        [JsonIgnore]
        [Required]
        [Column("roles")]
        public string RolesString { get; set; } = $"{Role.STANDARD}";
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; }
        [Column("password_hash")]
        public byte[] PasswordHash { get; set; }

        [NotMapped]
        public Role[] Roles
        {
            get { return RolesToRolesList(RolesString); }
            set { RolesString = RolesListToRoles(value); }
        }

        public static string RolesListToRoles(Role[] rolesList)
        {
            return string.Join(",", rolesList.Select(x => x.ToString()));
        }

        public static Role[] RolesToRolesList(string roles)
        {
            return roles.Split(",").Select(s => 
            {
                Role? role = null;
                if (Enum.TryParse(typeof(Role), s, out object o))
                {
                    role = (Role?)o;
                }
                return role;
            }).Where(x => x != null)
            .Select(x => x.Value).ToArray();
        }
    }
}
