using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Dtos
{
    public class UserRegisterDto
    {
        [MaxLength(100)]
        [MinLength(3)]
        [Required]
        public string Name { get; set;  }

        [MaxLength(320)]
        [MinLength(3)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [MaxLength(256)]
        [MinLength(8)]
        [Required]
        public string Password { get; set; }
    }
}
