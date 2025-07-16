using AutoMapper;
using UserManagement.API.DTOs;
using UserManagement.Domain.Entities;

namespace UserManagement.API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
        }
    }
}
