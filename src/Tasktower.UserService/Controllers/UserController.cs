using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasktower.Webtools.Security.Auth;
using Tasktower.UserService.Dtos;
using Tasktower.UserService.BusinessService;
using Microsoft.AspNetCore.Http;

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

        // GET api/<UserController>/{id}
        [HttpGet("/{id}")]
        public async Task<UserReadDto> GetUserById(Guid id)
        {
            var userData = new AuthContext(HttpContext).UserAuthData;
            var adminRoles = PermissionUtils.GetPermissionRoles(Permissions.READ_USER_SENSITVE);
            bool ignoreSensitive = userData == null || 
                !( userData.Roles.Any(r => adminRoles.Contains(r)) || userData.UserID.Equals(id));
            return await _userAccountService.GetUserByID(id, ignoreSensitive);
        }

        // GET api/<UserController>/userinfo
        [Authorize]
        [HttpGet("/userinfo")]
        public async Task<ActionResult<UserReadDto>> GetCurrentUserInfo()
        {
            var userData = new AuthContext(HttpContext).UserAuthData;
            return await _userAccountService.GetUserByID(userData.UserID, false);
        }

        // POST api/<UserController>/register
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("/register")]
        public async Task<ActionResult<UserReadDto>> RegisterUser([FromBody] UserRegisterDto dto)
        {
            var userReadDto = await _userAccountService.RegisterUser(dto);
            return CreatedAtAction(nameof(GetUserById), new { userReadDto.Id }, userReadDto);
        }

        // POST api/<UserController>/signin/standard
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("/signin/standard")]
        public async Task<ActionResult> SignIn([FromBody] UserStandardSignInDto dto)
        {
            var tokenDto = await _userAccountService.SignInStandard(dto);

            HttpContext.Response.Cookies.Append("REFRESH-TOKEN", tokenDto.RefreshToken, 
                new CookieOptions{HttpOnly = true, SameSite = SameSiteMode.Lax});
            HttpContext.Response.Cookies.Append("ACCESS-TOKEN", tokenDto.AccessToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });
            HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokenDto.XSRFToken,
                new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.Lax });
            return Ok();
        }
    }
}
