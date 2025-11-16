using Training.Models;
using Training.Dtos.User;

namespace Training.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<GetUserDto>> AddUser(AddUserDto user);
        Task<ServiceResponse<string>> DeleteUser(int id);
        Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser);
        Task<ServiceResponse<GetUserDto>> GetUserById(int id);
    }
}
