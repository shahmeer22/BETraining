using Microsoft.AspNetCore.Identity;
using Training.Models;

namespace Training.Repositories.IdentityRepository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public IdentityRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> CreateUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<SignInResult> CheckPasswordAsync(User user, string password, bool lockout)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, lockout);
        }
        public async Task<IdentityResult> DeleteAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }
        public async Task<string> GeneratePasswordResetToken(User newUser) { 
            return await _userManager.GeneratePasswordResetTokenAsync(newUser);
        }
        public async Task<IdentityResult> CreateUserWithoutPassword(User user)
        {
            return await _userManager.CreateAsync(user);
        }
        public async Task<User> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }
        public async Task<IdentityResult> UpdateAsync(User user) 
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
