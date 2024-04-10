using AutoMapper;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserResponseDto>().ReverseMap();
        CreateMap<User, UserByIdResponseDto>();
        CreateMap<UserCreateRequestDto, User>();
        CreateMap<UserUpdateRequestDto, User>();
    }
}