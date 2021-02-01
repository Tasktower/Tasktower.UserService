using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.UserService.Dtos;
using Tasktower.UserService.BusinessService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tasktower.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        public UserController(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        // POST api/<UserController>
        [HttpPost("/register")]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDto dto)
        {
            await _userAccountService.RegisterUser(dto);
            return NoContent();
        }
    }
}
