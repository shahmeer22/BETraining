using Microsoft.AspNetCore.Identity;
using Training.Models;

namespace Training.Repositories.IdentityRepository
{
    public interface IIdentityRepository
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<User> FindByEmailAsync(string email);
        Task<SignInResult> CheckPasswordAsync(User user, string password, bool lockout);
        Task<IdentityResult> DeleteAsync(User user);
        Task<string> GeneratePasswordResetToken(User newUser);
        Task<IdentityResult> CreateUserWithoutPassword(User user);
        Task<User> FindByIdAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword);
        Task<IdentityResult> UpdateAsync(User user);
    }
}
