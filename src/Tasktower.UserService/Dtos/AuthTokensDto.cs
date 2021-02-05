using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Dtos
{
    public class AuthTokensDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string XSRFToken { get; set; }
    }
}
