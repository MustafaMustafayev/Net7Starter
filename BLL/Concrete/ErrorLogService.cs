using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DAL.EntityFramework.Utility;
using DTO.ErrorLog;
using DTO.Responses;
using ENTITIES.Entities;

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

    public async Task<IResult> AddAsync(ErrorLogCreateDto dto)
    {
        var data = _mapper.Map<ErrorLog>(dto);

        await _unitOfWork.ErrorLogRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<ErrorLogResponseDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.ErrorLogRepository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<ErrorLog>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<ErrorLogResponseDto>(_mapper.Map<List<ErrorLogResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);

        return new SuccessDataResult<PaginatedList<ErrorLogResponseDto>>(responseDto, Messages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<ErrorLogResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<ErrorLogResponseDto>>(await _unitOfWork.ErrorLogRepository.GetListAsync());

        return new SuccessDataResult<IEnumerable<ErrorLogResponseDto>>(datas, Messages.Success.Translate());
    }

    public async Task<IDataResult<ErrorLogResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<ErrorLogResponseDto>(await _unitOfWork.ErrorLogRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<ErrorLogResponseDto>(data, Messages.Success.Translate());
    }
}