using AutoMapper;
using Project.DTO.DTOs.OrganizationDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Mappers
{
	public class OrganizationMapper : Profile
	{
		public OrganizationMapper()
		{
			CreateMap<Organization, OrganizationToListDTO>();
			CreateMap<OrganizationToAddOrUpdateDTO, Organization>();
		}
	}
}

