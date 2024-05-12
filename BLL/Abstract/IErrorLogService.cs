using DAL.EntityFramework.Utility;
using DTO.ErrorLog;
using DTO.Responses;

namespace BLL.Abstract;

public interface IErrorLogService
{
    Task<IDataResult<PaginatedList<ErrorLogResponseDto>>> GetAsPaginatedListAsync();
    Task<IDataResult<IEnumerable<ErrorLogResponseDto>>> GetAsync();
    Task<IDataResult<ErrorLogResponseDto>> GetAsync(Guid id);
    Task<IResult> AddAsync(ErrorLogCreateDto dto);
}