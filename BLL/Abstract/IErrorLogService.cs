using DTO.ErrorLog;
using DTO.Responses;
using DAL.EntityFramework.Utility;

namespace BLL.Abstract;

public interface IErrorLogService
{
    Task<IDataResult<PaginatedList<ErrorLogToListDto>>> GetAsPaginatedListAsync();
    Task<IDataResult<List<ErrorLogToListDto>>> GetAsync();
    Task<IDataResult<ErrorLogToListDto>> GetAsync(Guid id);
    Task<IResult> AddAsync(ErrorLogToAddDto dto);
}
