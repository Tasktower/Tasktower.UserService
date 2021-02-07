using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Security.Auth.Middleware
{
    public class JWTMiddlewareOptions<TKeyAccessor>
    {
        public Func<string, TKeyAccessor, Task<string>> KeyRetrieverAsync { get; set; }
    }
}
