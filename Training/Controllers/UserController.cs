using Microsoft.AspNetCore.Mvc;
using Training.Dtos.User;
using Training.Models;
using Training.Services.UserService;

namespace Training.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserDto user) {
            ServiceResponse res = await _userService.AddUser(user);
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
    }
}
