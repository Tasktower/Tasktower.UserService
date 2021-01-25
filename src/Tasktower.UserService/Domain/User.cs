using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        public string Roles { get; set; }
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; }
        [Column("password_hash")]
        public byte[] PasswordHash { get; set; }

        [NotMapped]
        public string[] RolesList { 
            get { return RolesToRolesList(Roles); } 
            set { Roles = RolesListToRoles(value);  } 
        }

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
