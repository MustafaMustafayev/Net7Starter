using AutoMapper;
using DTO.Organization;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class OrganizationMapper : Profile
{
    public OrganizationMapper()
    {
        CreateMap<Organization, OrganizationResponseDto>();
        CreateMap<Organization, OrganizationByIdResponseDto>();
        CreateMap<OrganizationCreateRequestDto, Organization>();
        CreateMap<OrganizationUpdateRequestDto, Organization>();
    }
}