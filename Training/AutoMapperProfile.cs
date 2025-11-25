using AutoMapper;
using Training.Dtos.User;
using Training.Models;

namespace Training
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<RegisterUserDto, User>();
            CreateMap<AddUserDto, User>();
        }
    }
}
