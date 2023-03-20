using AutoMapper;
using DTO.Auth;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserToListDto>().ReverseMap();
        CreateMap<UserToAddDto, User>();
        CreateMap<UserToUpdateDto, User>();
        CreateMap<Photo, PhotoToListDto>();
        CreateMap<UserToListDto, UserResponseDto>();
        CreateMap<UserResponseDto, User>();
    }
}