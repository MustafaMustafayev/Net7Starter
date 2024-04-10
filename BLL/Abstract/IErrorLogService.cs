using DTO.ErrorLog;
using DTO.Responses;
using DAL.EntityFramework.Utility;

namespace BLL.Abstract;

public interface IErrorLogService
{
    Task<IDataResult<PaginatedList<ErrorLogResponseDto>>> GetAsPaginatedListAsync();
    Task<IDataResult<List<ErrorLogResponseDto>>> GetAsync();
    Task<IDataResult<ErrorLogResponseDto>> GetAsync(Guid id);
    Task<IResult> AddAsync(ErrorLogCreateDto dto);
}
