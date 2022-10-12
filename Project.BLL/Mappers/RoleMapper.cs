using AutoMapper;
using Project.DTO.DTOs.RoleDto;
using Project.Entity.Entities;

namespace Project.BLL.Mappers;

public class RoleMapper : Profile
{
    public RoleMapper()
    {
        CreateMap<Role, RoleToListDto>();
        CreateMap<RoleToAddOrUpdateDto, Role>();
    }
}