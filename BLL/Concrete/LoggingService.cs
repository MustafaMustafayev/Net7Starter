using AutoMapper;
using BLL.Abstract;
using DAL.EntityFramework.UnitOfWork;
using DTO.Logging;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class LoggingService : ILoggingService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LoggingService(IUnitOfWork unitOfWork,
                          IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddLogAsync(RequestLogDto dto)
    {
        var data = _mapper.Map<RequestLog>(dto);
        await _unitOfWork.RequestLogRepository.AddRequestLogAsync(data);
        await _unitOfWork.CommitAsync();
    }
}