using Training.Models;
using Training.Dtos.User;

namespace Training.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse> LoginUser(LoginUserDto login);
        Task<ServiceResponse> RegisterUser(RegisterUserDto user);
        Task<ServiceResponse> DeleteUser(int id);
        Task<ServiceResponse> UpdateUser(UpdateUserDto updatedUser);
        Task<ServiceResponse> GetUserById(int id);
        Task<ServiceResponse> GetUsers(PaginationQueryParams param);
        Task<ServiceResponse> CreateUser(AddUserDto user);
        Task<ServiceResponse> SetPassword(string userId,
                                          string token,
                                          string newPassword,
                                          string confirmPassword);
    }
}
