using AutoMapper;
using Project.DTO.DTOs.RoleDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Mappers
{
	public class RoleMapper : Profile
	{
		public RoleMapper()
		{
			CreateMap<Role, RoleToListDTO>();
			CreateMap<RoleToAddOrUpdateDTO, Role>();
		}
	}
}

