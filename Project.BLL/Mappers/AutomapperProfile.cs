using AutoMapper;
using Project.DTO.DTOs.CustomLoggingDto;
using Project.DTO.DTOs.OrganizationDto;
using Project.DTO.DTOs.RoleDto;
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