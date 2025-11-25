using System.ComponentModel.DataAnnotations;

namespace Training.Dtos.User
{
    public class AddUserDto
    {
        [Required]
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
