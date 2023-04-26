using AutoMapper;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserToListDto>()
            .ForCtorParam(nameof(UserToListDto.ProfileFileHashName), opt => opt.MapFrom(src => src.ProfileFile == null ? null : src.ProfileFile.HashName));
        CreateMap<UserToAddDto, User>();
        CreateMap<UserToUpdateDto, User>();
        CreateMap<UserToListDto, User>();
    }
}