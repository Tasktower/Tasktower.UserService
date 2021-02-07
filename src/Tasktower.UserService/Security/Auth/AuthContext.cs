using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Security.Auth.AuthData;

namespace Tasktower.UserService.Security.Auth

{
    public class AuthContext
    {
        private readonly HttpContext _context;
        public AuthContext(HttpContext context)
        {
            _context = context;
        }

        public UserAuthData UserAuthData => (UserAuthData)_context.Items["UserData"];
        public string XSRFToken => (string)_context.Items["XSRFToken"];

    }
}
