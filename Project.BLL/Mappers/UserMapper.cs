using AutoMapper;
using Project.DTO.DTOs.UserDto;
using Project.Entity.Entities;

namespace Project.BLL.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<UserToAddDto, User>();
        CreateMap<UserToUpdateDto, User>();
        CreateMap<User, UserToListDto>();
    }
}