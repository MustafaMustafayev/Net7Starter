using AutoMapper;
using Project.DTO.DTOs.CustomLoggingDTOs;
using Project.DTO.DTOs.OrganizationDTOs;
using Project.DTO.DTOs.RoleDTOs;
using Project.DTO.DTOs.UserDto;
using Project.Entity.Entities;

namespace Project.BLL.Mappers;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<ResponseLogDto, ResponseLog>();
        CreateMap<RequestLogDto, RequestLog>();

        CreateMap<Organization, OrganizationToListDto>();
        CreateMap<OrganizationToAddOrUpdateDto, Organization>();

        CreateMap<Role, RoleToListDto>();
        CreateMap<RoleToAddOrUpdateDto, Role>();

        CreateMap<UserToAddDto, User>();
        CreateMap<UserToUpdateDto, User>();
        CreateMap<User, UserToListDto>();
    }
}