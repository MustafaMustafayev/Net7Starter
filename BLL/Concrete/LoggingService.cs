using AutoMapper;
using BLL.Abstract;
using DAL.EntityFramework.Abstract;
using DTO.Logging;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class LoggingService(IMapper mapper,
                            IRequestLogRepository requestLogRepository) : ILoggingService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRequestLogRepository _requestLogRepository = requestLogRepository;

    public async Task AddLogAsync(RequestLogDto dto)
    {
        var data = _mapper.Map<RequestLog>(dto);
        await _requestLogRepository.AddAsync(data);
    }
}