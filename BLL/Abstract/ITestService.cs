using DTO.Test;
using DTO.Responses;
using DAL.EntityFramework.Utility;

namespace BLL.Abstract;

public interface ITestService
{
    Task<IDataResult<PaginatedList<TestToListDto>>> GetAsPaginatedListAsync();
    Task<IDataResult<List<TestToListDto>>> GetAsync();
    Task<IDataResult<TestToListDto>> GetAsync(Guid id);
    Task<IResult> AddAsync(TestToAddDto dto);
    Task<IResult> UpdateAsync(Guid id, TestToUpdateDto dto);
    Task<IResult> SoftDeleteAsync(Guid id);
}
