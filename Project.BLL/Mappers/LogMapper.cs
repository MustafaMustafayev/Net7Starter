using AutoMapper;
using Project.DTO.CustomLogging;
using Project.Entity.Entities;

namespace Project.BLL.Mappers;

public class LogMapper : Profile
{
    public LogMapper()
    {
        CreateMap<ResponseLogDto, ResponseLog>();
        CreateMap<RequestLogDto, RequestLog>();
    }
}