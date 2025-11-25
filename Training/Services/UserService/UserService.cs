using Training.Dtos.User;
using Training.Data;
using Training.Models;
using Microsoft.EntityFrameworkCore;
using Training.Constants;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Training.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public UserService(IMapper mapper, DataContext context, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<ServiceResponse> RegisterUser(RegisterUserDto user)
        {
            ServiceResponse res = new();
            try
            {
                User newUser = _mapper.Map<User>(user);

                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                if (!result.Succeeded) {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                res.Message = ResponseMessages.USER_CREATED_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ServiceResponse> LoginUser(LoginUserDto login)
        {
            ServiceResponse res = new();
            try
            {
                User? user = await _userManager.FindByEmailAsync(login.Email);

                if (user == null)
                {
                    throw new Exception(ResponseMessages.USER_DOES_NOT_EXIST);
                }

                SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

                if (!result.Succeeded)
                {
                    throw new Exception(ResponseMessages.INVALID_CREDENTIALS);
     
                }

                res.Data = CreateToken(user);
                res.Message = ResponseMessages.SUCCESSFULLY_LOGIN;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ServiceResponse> DeleteUser(int id)
        {
            ServiceResponse res = new();
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);

                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }

                IdentityResult result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                res.Message = ResponseMessages.SUCCESSFULLY_DELETED;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ServiceResponse> UpdateUser(UpdateUserDto updatedUser)
        {
            ServiceResponse res = new();
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(c => c.Id == updatedUser.Id);
                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Age = updatedUser.Age;
                user.PhoneNumber = updatedUser.PhoneNumber;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                res.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ServiceResponse> GetUserById(int id)
        {
            ServiceResponse res = new();
            try
            {
                User? user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }
                res.Data = _mapper.Map<GetUserDto>(user);
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            SymmetricSecurityKey key = new(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
