using AutoMapper;
using Project.DTO.DTOs.UserDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Mappers
{
	public class UserMapper : Profile
	{
		public UserMapper()
		{
			CreateMap<UserToAddDTO, User>();
			CreateMap<UserToUpdateDTO, User>();
			CreateMap<User, UserToListDTO>();
		}
	}
}

