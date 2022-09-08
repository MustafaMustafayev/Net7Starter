using AutoMapper;
using Project.DTO.DTOs.CustomLoggingDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Mappers
{
	public class LogMapper : Profile
	{
		public LogMapper()
		{
			CreateMap<ResponseLogDTO, ResponseLog>();
			CreateMap<RequestLogDTO, RequestLog>();
		}
	}
}

