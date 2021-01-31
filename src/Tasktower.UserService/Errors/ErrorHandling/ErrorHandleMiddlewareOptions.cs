using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasktower.UserService.Errors.ErrorHandling
{
    public class ErrorHandleMiddlewareOptions
    {
        public bool UseStackTrace { get; set; }
        public bool ShowAllErrorMessages { get; set; }
    }
}
