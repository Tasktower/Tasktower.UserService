using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security.Auth
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        DEFAULT = 0, SUPERUSER, ADMINISTRATOR, STANDARD
    }

    public static class RoleGroups
    {
        public static ISet<Role> AdminRoles()
        {
            return new HashSet<Role> { Role.SUPERUSER, Role.SUPERUSER };
        }

        public static ISet<Role> StandardRoles()
        {
            return new HashSet<Role> { Role.STANDARD };
        }
    }
}
