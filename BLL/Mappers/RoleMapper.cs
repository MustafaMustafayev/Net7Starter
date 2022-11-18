using AutoMapper;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleToListDto>();
        CreateMap<RoleToAddDto, Role>();
        CreateMap<RoleToUpdateDto, Role>();
    }
}