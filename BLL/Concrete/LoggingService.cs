using AutoMapper;
using BLL.Abstract;
using DAL.EntityFramework.UnitOfWork;
using DTO.Logging;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class LoggingService(IMapper mapper,
                            IUnitOfWork unitOfWork) : ILoggingService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task AddLogAsync(RequestLogDto dto)
    {
        var data = _mapper.Map<RequestLog>(dto);
        await _unitOfWork.RequestLogRepository.AddRequestLogAsync(data);
        await _unitOfWork.CommitAsync();
    }
}