using AutoMapper;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleToListDto>().ReverseMap();
        CreateMap<Role, RoleToFkDto>();
        CreateMap<RoleToAddDto, Role>();
        CreateMap<RoleToUpdateDto, Role>();
    }
}