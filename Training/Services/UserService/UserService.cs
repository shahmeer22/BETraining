using Training.Dtos.User;
using Training.Data;
using Training.Models;

namespace Training.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<GetUserDto>> AddUser(AddUserDto user)
        {
            ServiceResponse<GetUserDto> res = new();
            try
            {
                User newUser = new User {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    phoneNumber = user.phoneNumber
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                res.Data = new GetUserDto
                {
                    Id = newUser.Id,        
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Age = newUser.Age,
                    phoneNumber = newUser.phoneNumber

                };

                res.Message = "User created successfully.";
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ServiceResponse<string>> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
