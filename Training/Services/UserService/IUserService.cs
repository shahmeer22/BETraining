using Training.Models;
using Training.Dtos.User;

namespace Training.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse> AddUser(AddUserDto user);
        Task<ServiceResponse> DeleteUser(int id);
        Task<ServiceResponse> UpdateUser(UpdateUserDto updatedUser);
        Task<ServiceResponse> GetUserById(int id);
    }
}
