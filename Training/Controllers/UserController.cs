using Microsoft.AspNetCore.Mvc;
using Training.Dtos.User;
using Training.Models;
using Training.Services.UserService;

namespace Training.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto user) {
            ServiceResponse <GetUserDto> res = await _userService.AddUser(user);
            if (!res.Success) {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ServiceResponse<string> res = await _userService.DeleteUser(id);
            if (!res.Success)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }
    }
}
