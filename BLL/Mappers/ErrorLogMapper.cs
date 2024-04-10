using AutoMapper;
using DTO.ErrorLog;
using ENTITIES.Entities;

namespace BLL.Mappers;

public class ErrorLogMapper : Profile
{
    public ErrorLogMapper()
    {
        CreateMap<ErrorLog, ErrorLogResponseDto>();
        CreateMap<ErrorLogCreateDto, ErrorLog>();
    }
}
