using AutoMapper;
using BLL.Abstract;
using DAL.UnitOfWorks.Abstract;
using DTO.CustomLogging;
using ENTITIES.Entities.Logging;

namespace BLL.Concrete;

public class LoggingService : ILoggingService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public LoggingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task AddLogAsync(RequestLogDto requestLogDto)
    {
        var entity = _mapper.Map<RequestLog>(requestLogDto);
        await _unitOfWork.LoggingRepository.AddLogAsync(entity);
        await _unitOfWork.CommitAsync();
    }
}