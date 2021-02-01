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
        SUPERUSER, STANDARD
    }
}
