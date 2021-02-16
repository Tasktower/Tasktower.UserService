using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tasktower.UserService.Domain
{
    [Table("user_profiles")]
    public class UserProfile : AbstractDomain
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name")]
        public string Name { get; set; }
    }
}
