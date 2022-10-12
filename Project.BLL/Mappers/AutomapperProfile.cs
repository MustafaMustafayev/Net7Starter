using AutoMapper;
using Project.DTO.CustomLogging;
using Project.DTO.Organization;
using Project.DTO.Role;
using Project.DTO.User;
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