using DAL.EntityFramework.Utility;
using DTO.Permission;
using DTO.Responses;

namespace BLL.Abstract;

public interface IPermissionService
{
    Task<IDataResult<List<PermissionResponseDto>>> GetAsync();

    Task<IDataResult<PaginatedList<PermissionResponseDto>>> GetAsPaginatedListAsync();

    Task<IDataResult<PermissionResponseDto>> GetAsync(Guid id);

    Task<IResult> AddAsync(PermissionCreateRequestDto dto);

    Task<IResult> UpdateAsync(Guid permissionId, PermissionUpdateRequestDto dto);

    Task<IResult> SoftDeleteAsync(Guid id);
}