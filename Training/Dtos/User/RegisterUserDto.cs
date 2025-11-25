using System.ComponentModel.DataAnnotations;

namespace Training.Dtos.User
{
    public class RegisterUserDto : AddUserDto
    {
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "password do not match")]
        public string ConfirmPassword { get; set; } 
    }
}
