using AutoMapper;
using Project.DTO.DTOs.OrganizationDto;
using Project.Entity.Entities;

namespace Project.BLL.Mappers;

public class OrganizationMapper : Profile
{
    public OrganizationMapper()
    {
        CreateMap<Organization, OrganizationToListDto>();
        CreateMap<OrganizationToAddOrUpdateDto, Organization>();
    }
}