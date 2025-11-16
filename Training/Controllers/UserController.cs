using Microsoft.AspNetCore.Mvc;
using Training.Dtos.User;
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
            return Ok(await _userService.AddUser(user));
        }
    }
}
