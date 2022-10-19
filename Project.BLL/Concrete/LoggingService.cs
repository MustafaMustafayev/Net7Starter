using AutoMapper;
using Project.BLL.Abstract;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.CustomLogging;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

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