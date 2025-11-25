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
using Training.Repositories.GenericRepository;
using Training.Repositories.IdentityRepository;
using System.Net.Mail;
using Training.Helper;

namespace Training.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IIdentityRepository _identityRepository;
        private readonly IGenericRepository<User> _genericRepository;
        public UserService(IMapper mapper, IConfiguration configuration, IGenericRepository<User> genericRepository, 
            IIdentityRepository identityRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _genericRepository = genericRepository;
            _identityRepository = identityRepository;
        }

        public async Task<ServiceResponse> RegisterUser(RegisterUserDto user)
        {
            ServiceResponse res = new();
            try
            {
                User newUser = _mapper.Map<User>(user);
                newUser.EmailConfirmed = true;
                IdentityResult result = await _identityRepository.CreateUser(newUser, user.Password);

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
                User? user = await _identityRepository.FindByEmailAsync(login.Email);

                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }

                if (!user.EmailConfirmed) 
                {
                    throw new Exception(ExceptionMessages.EMAIL_VERIFICATION_PENDING);
                }

                SignInResult result = await _identityRepository.CheckPasswordAsync(user, login.Password, false);

                if (!result.Succeeded)
                {
                    throw new Exception(ExceptionMessages.INVALID_CREDENTIALS);
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

        public async Task<ServiceResponse> CreateUser(AddUserDto user)
        {
            string confirmationLink = _configuration["SMTPSettings:ConfirmLink"];

            ServiceResponse res = new();
            try
            {
                User newUser = _mapper.Map<User>(user);

                IdentityResult result = await _identityRepository.CreateUserWithoutPassword(newUser);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                string token = await _identityRepository.GeneratePasswordResetToken(newUser);
                token = System.Web.HttpUtility.UrlEncode(token);

                string link = $"{confirmationLink}?userId={newUser.Id}&token={token}";

                await SendEmail(newUser, link);

                res.Message = ResponseMessages.USER_CREATED_SUCCESSFULLY;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ServiceResponse> SetPassword(string userId,
                                                       string token,
                                                       string newPassword,
                                                       string confirmPassword) 
        {
            ServiceResponse res = new();
            try
            {
                if (newPassword != confirmPassword)
                {
                    throw new Exception(ExceptionMessages.PASSWORD_MISMATCH);
                }

                User user = await _identityRepository.FindByIdAsync(userId);

                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }

                user.EmailConfirmed = true;
                
                IdentityResult updateResult = await _identityRepository.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    throw new Exception(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                }

                IdentityResult result = await _identityRepository.ResetPasswordAsync(user, token, newPassword);

                if (!result.Succeeded)
                {
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

        public async Task<ServiceResponse> DeleteUser(int id)
        {
            ServiceResponse res = new();
            try
            {
                User? user = await _genericRepository.FirstOrDefaultAsync(c => c.Id == id);

                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }

                IdentityResult result = await _identityRepository.DeleteAsync(user);

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
                User? user = await _genericRepository.FirstOrDefaultAsync(c => c.Id == updatedUser.Id);
                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Age = updatedUser.Age;
                user.PhoneNumber = updatedUser.PhoneNumber;
                _genericRepository.Update(user);
                await _genericRepository.SaveChangesAsync();
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
                User? user = await _genericRepository.FirstOrDefaultAsync(c => c.Id == id);
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

        public async Task<ServiceResponse> GetUsers(PaginationQueryParams param)
        {
            ServiceResponse res = new();
            try
            {
                (IQueryable<User> query, int totalItems) = await _genericRepository.GetPaginatedResult(_genericRepository.Query(),
                                                                                                        param);
                List<User> users = await query.ToListAsync();

                PaginatedResult<GetUserDto> paginatedResult = new()
                {
                    Items = users.Select(x => _mapper.Map<GetUserDto>(x)).ToList(),
                    Page = param.Page,
                    PageSize = param.PageSize,
                    TotalCount = totalItems
                };

                res.Data = paginatedResult;
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
            string secretKey = _configuration["AppSettings:SecretKey"];
            int expiry = int.Parse(_configuration["AppSettings:TokenExpiryInDays"]);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secretKey));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(expiry),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task SendEmail(User newUser, string link) {
            string sender = _configuration["SMTPSettings:Sender"];
            string client = _configuration["SMTPSettings:SMTPClient"];
            string password = _configuration["SMTPSettings:Password"];
            int port = int.Parse(_configuration["SMTPSettings:Port"]);
            bool ssl = bool.Parse(_configuration["SMTPSettings:EnableSSL"]);

            MailMessage message = new();
            message.From = new MailAddress(sender);
            message.To.Add(new MailAddress(newUser.Email));
            message.Subject = "Set your password";
            message.Body = HtmlTemplates.GetEmailBody(newUser.FirstName, link);

            message.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient(client)
            {
                Port = port,
                Credentials = new System.Net.NetworkCredential(sender, password),
                EnableSsl = ssl
            };

            await smtp.SendMailAsync(message);
        }
    }
}
