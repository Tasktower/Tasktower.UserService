using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        DEFAULT=0, SUPERUSER, ADMINISTRATOR, STANDARD
    }

    public static class RoleGroups 
    {
        public static IEnumerable<Role> AdminRoles()
        {
            return new HashSet<Role> { Role.SUPERUSER, Role.SUPERUSER };
        }

        public static IEnumerable<Role> StandardRoles()
        {
            return new HashSet<Role> { Role.STANDARD };
        }
    }
}
