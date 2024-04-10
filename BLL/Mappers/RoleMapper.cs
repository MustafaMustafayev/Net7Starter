using AutoMapper;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleResponseDto>();
        CreateMap<Role, RoleByIdResponseDto>();
        CreateMap<Role, RoleToFkDto>();
        CreateMap<RoleCreateRequestDto, Role>();
        CreateMap<RoleUpdateRequestDto, Role>();
    }
}