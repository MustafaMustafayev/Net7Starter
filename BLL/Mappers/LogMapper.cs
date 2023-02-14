using AutoMapper;
using DTO.Logging;
using ENTITIES.Entities.Logging;

namespace BLL.Mappers;

public class LogMapper : Profile
{
    public LogMapper()
    {
        CreateMap<ResponseLogDto, ResponseLog>();
        CreateMap<RequestLogDto, RequestLog>();
    }
}