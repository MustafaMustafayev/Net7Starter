using AutoMapper;
using DTO.ErrorLog;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class ErrorLogMapper : Profile
{
    public ErrorLogMapper()
    {
        CreateMap<ErrorLog, ErrorLogResponseDto>()
            .ForMember(dest => dest.DateTime, opt => opt.MapFrom(src => src.DateTime.ToString("dd/MM/yyyy HH:mm")));

        CreateMap<ErrorLogCreateDto, ErrorLog>();
    }
}