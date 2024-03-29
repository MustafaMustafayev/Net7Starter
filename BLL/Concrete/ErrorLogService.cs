using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DTO.Responses;
using DTO.ErrorLog;
using ENTITIES.Entities;
using DAL.EntityFramework.Utility;
using DAL.EntityFramework.UnitOfWork;

namespace BLL.Concrete;

public class ErrorLogService : IErrorLogService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;
    public ErrorLogService(IMapper mapper, IUnitOfWork unitOfWork, IUtilService utilService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(ErrorLogToAddDto dto)
    {
        var data = _mapper.Map<ErrorLog>(dto);

        await _unitOfWork.ErrorLogRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<ErrorLogToListDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.ErrorLogRepository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<ErrorLog>.CreateAsync(datas.OrderBy(m => m.ErrorLogId), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<ErrorLogToListDto>(_mapper.Map<List<ErrorLogToListDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);

        return new SuccessDataResult<PaginatedList<ErrorLogToListDto>>(responseDto, Messages.Success.Translate());
    }

    public async Task<IDataResult<List<ErrorLogToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<ErrorLogToListDto>>(await _unitOfWork.ErrorLogRepository.GetListAsync());

        return new SuccessDataResult<List<ErrorLogToListDto>>(datas, Messages.Success.Translate());
    }

    public async Task<IDataResult<ErrorLogToListDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<ErrorLogToListDto>(await _unitOfWork.ErrorLogRepository.GetAsync(m => m.ErrorLogId == id));

        return new SuccessDataResult<ErrorLogToListDto>(data, Messages.Success.Translate());
    }
}
