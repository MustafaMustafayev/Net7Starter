using AutoMapper;
using Project.DTO.Role;
using Project.Entity.Entities;

namespace Project.BLL.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleToListDto>();
        CreateMap<RoleToAddDto, Role>();
        CreateMap<RoleToUpdateDto, Role>();
    }
}