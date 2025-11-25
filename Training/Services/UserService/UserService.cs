using Training.Dtos.User;
using Training.Data;
using Training.Models;
using Microsoft.EntityFrameworkCore;
using Training.Constants;

namespace Training.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> AddUser(AddUserDto user)
        {
            ServiceResponse res = new();
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
                User? user = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
                if (user == null)
                {
                    throw new Exception(ExceptionMessages.USER_DOES_NOT_EXIST);
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
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
                user.phoneNumber = updatedUser.phoneNumber;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                res.Data = new GetUserDto
                {   
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age= user.Age,
                    phoneNumber = user.phoneNumber
                };
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
                res.Data = new GetUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Age = user.Age,
                    phoneNumber = user.phoneNumber
                };
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message;
            }
            return res;
        }
    }
}
