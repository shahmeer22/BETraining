using Training.Dtos.User;
using Training.Models;

namespace Training.Services.UserService
{
    public class UserService : IUserService
    {
        public Task<ServiceResponse<GetUserDto>> AddUser(AddUserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<string>> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<GetUserDto>> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<GetUserDto>> UpdateUser(UpdateUserDto updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
