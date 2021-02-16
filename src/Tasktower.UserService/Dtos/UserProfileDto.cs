using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Tasktower.UserService.Domain;

namespace Tasktower.UserService.Dtos
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; }

        public static UserProfileDto FromUser(UserProfile user, bool ignoreSensitive = true)
        {
            var config = TypeAdapterConfig<UserProfile, UserProfileDto>.NewConfig()
                .IgnoreIf((u, d) => ignoreSensitive, 
                    d => d.CreatedAt, d => d.UpdatedAt)
                .Config;
            return user.Adapt<UserProfileDto>(config);
        }
    }
}
