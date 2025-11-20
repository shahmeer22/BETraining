using Training.Dtos.User;
using Microsoft.AspNetCore.Identity;

namespace Training.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
