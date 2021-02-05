using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Tasktower.UserService.Domain;
using Tasktower.UserService.Security;

namespace Tasktower.UserService.Dtos
{
    public class UserReadDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public bool? EmailVerified { get; set; }

        public Role[] Roles { get; set; }

        public static UserReadDto FromUser(User user, bool ignoreSensitive = true)
        {
            var config = TypeAdapterConfig<User, UserReadDto>.NewConfig()
                .IgnoreIf((u, d) => ignoreSensitive, 
                    d => d.Roles, d => d.EmailVerified, d => d.CreatedAt, d => d.UpdatedAt)
                .Config;
            return user.Adapt<UserReadDto>(config);
        }
    }
}
