using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Utility;
using DTO.ErrorLog;
using DTO.Responses;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class ErrorLogService(IMapper mapper,
                             IErrorLogRepository errorLogRepository) : IErrorLogService
{
    private readonly IMapper _mapper = mapper;
    private readonly IErrorLogRepository _errorLogRepository = errorLogRepository;

    public async Task<IResult> AddAsync(ErrorLogCreateDto dto)
    {
        var data = _mapper.Map<ErrorLog>(dto);
        await _errorLogRepository.AddAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<ErrorLogResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _errorLogRepository.GetList();

        var response = await PaginatedList<ErrorLog>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<ErrorLogResponseDto>(_mapper.Map<List<ErrorLogResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<ErrorLogResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<ErrorLogResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<ErrorLogResponseDto>>(await _errorLogRepository.GetListAsync());

        return new SuccessDataResult<IEnumerable<ErrorLogResponseDto>>(datas, EMessages.Success.Translate());
    }

    public async Task<IDataResult<ErrorLogResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<ErrorLogResponseDto>(await _errorLogRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<ErrorLogResponseDto>(data, EMessages.Success.Translate());
    }
}