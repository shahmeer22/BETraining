using System.ComponentModel.DataAnnotations;

namespace Training.Dtos.User
{
    public class RegisterUserDto
    {
        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "password do not match")]
        public string ConfirmPassword { get; set; } 
    }
}
