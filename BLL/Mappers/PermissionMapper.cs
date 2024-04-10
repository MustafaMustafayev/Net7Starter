using AutoMapper;
using DTO.Permission;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class PermissionMapper : Profile
{
    public PermissionMapper()
    {
        CreateMap<Permission, PermissionResponseDto>();
        CreateMap<Permission, PermissionByIdResponseDto>();
        CreateMap<PermissionCreateRequestDto, Permission>();
        CreateMap<PermissionUpdateRequestDto, Permission>();
    }
}