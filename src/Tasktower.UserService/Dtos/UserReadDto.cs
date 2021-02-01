using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Domain;
using Tasktower.UserService.Security;

namespace Tasktower.UserService.Dtos
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public Role[] Roles { get; set; }

        public static UserReadDto FromUser(User user)
        {
            return new UserReadDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Name = user.Name,
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                Roles = user.Roles
            };
        }
    }
}
