using AutoMapper;
using DTO.Organization;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class OrganizationMapper : Profile
{
    public OrganizationMapper()
    {
        CreateMap<Organization, OrganizationToListDto>();
        CreateMap<OrganizationToAddDto, Organization>();
        CreateMap<OrganizationToUpdateDto, Organization>();
    }
}