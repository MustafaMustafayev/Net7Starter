using AutoMapper;
using DTO.Permission;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class PermissionMapper : Profile
{
    public PermissionMapper()
    {
        CreateMap<PermissionToAddDto, Permission>();
        CreateMap<PermissionToUpdateDto, Permission>();
        CreateMap<Permission, PermissionToListDto>();
    }
}