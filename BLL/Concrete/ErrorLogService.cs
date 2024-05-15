using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DAL.EntityFramework.Utility;
using DTO.ErrorLog;
using DTO.Responses;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class ErrorLogService(IMapper mapper,
                             IUnitOfWork unitOfWork) : IErrorLogService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IResult> AddAsync(ErrorLogCreateDto dto)
    {
        var data = _mapper.Map<ErrorLog>(dto);

        await _unitOfWork.ErrorLogRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<ErrorLogResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _unitOfWork.ErrorLogRepository.GetList();

        var response = await PaginatedList<ErrorLog>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<ErrorLogResponseDto>(_mapper.Map<List<ErrorLogResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<ErrorLogResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<ErrorLogResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<ErrorLogResponseDto>>(await _unitOfWork.ErrorLogRepository.GetListAsync());

        return new SuccessDataResult<IEnumerable<ErrorLogResponseDto>>(datas, EMessages.Success.Translate());
    }

    public async Task<IDataResult<ErrorLogResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<ErrorLogResponseDto>(await _unitOfWork.ErrorLogRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<ErrorLogResponseDto>(data, EMessages.Success.Translate());
    }
}