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
        DEFAULT = 0, 
        SUPERUSER, 
        ADMINISTRATOR, 
        STANDARD, 
        USER_SENSITIVE_READER, 
        USER_SENSITIVE_WRITER
    }
}
