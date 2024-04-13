using DTO.Department;
using DTO.Responses;
using DAL.EntityFramework.Utility;

namespace BLL.Abstract;

public interface IDepartmentService
{
    Task<IDataResult<PaginatedList<DepartmentResponseDto>>> GetAsPaginatedListAsync();
    Task<IDataResult<IEnumerable<DepartmentResponseDto>>> GetAsync();
    Task<IDataResult<DepartmentByIdResponseDto>> GetAsync(Guid id);
    Task<IResult> AddAsync(DepartmentCreateRequestDto dto);
    Task<IResult> UpdateAsync(Guid id, DepartmentUpdateRequestDto dto);
    Task<IResult> SoftDeleteAsync(Guid id);
}
