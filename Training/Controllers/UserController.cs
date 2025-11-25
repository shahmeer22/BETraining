using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training.Dtos.User;
using Training.Helper;
using Training.Models;
using Training.Services.UserService;

namespace Training.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController: BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto user) {
            ServiceResponse res = await _userService.RegisterUser(user);
            if (!res.Success) 
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginUserDto login)
        {
            ServiceResponse res = await _userService.LoginUser(login);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ServiceResponse res = await _userService.DeleteUser(id);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updatedUser)
        {
            ServiceResponse res = await _userService.UpdateUser(updatedUser);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            ServiceResponse res = await _userService.GetUserById(id);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationQueryParams param)
        {
            ServiceResponse res = await _userService.GetUsers(param);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser(AddUserDto newUser)
        {
            ServiceResponse res = await _userService.CreateUser(newUser);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [AllowAnonymous]
        [HttpPost("SetPassword")]
        public async Task<IActionResult> SetPassword([FromForm] string userId,
                                                     [FromForm] string token,
                                                     [FromForm] string newPassword,
                                                     [FromForm] string confirmPassword)
        {
            ServiceResponse res = await _userService.SetPassword(userId, token, newPassword, confirmPassword);
            return Content(res.Message);
        }

        [AllowAnonymous]
        [HttpGet("SetPasswordPage")]
        public IActionResult SetPasswordPage([FromQuery] string userId, [FromQuery] string token)
        {
            string html = HtmlTemplates.GetSetPasswordPage(userId, token, "/user/setpassword");
            return Content(html, "text/html");
        }
    }
}
