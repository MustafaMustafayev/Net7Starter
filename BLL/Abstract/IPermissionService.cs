using DAL.EntityFramework.Utility;
using DTO.Permission;
using DTO.Responses;

namespace BLL.Abstract;

public interface IPermissionService
{
    Task<IDataResult<List<PermissionToListDto>>> GetAsync();

    Task<IDataResult<PaginatedList<PermissionToListDto>>> GetAsPaginatedListAsync();

    Task<IDataResult<PermissionToListDto>> GetAsync(int id);

    Task<IResult> AddAsync(PermissionToAddDto dto);

    Task<IResult> UpdateAsync(int permissionId, PermissionToUpdateDto dto);

    Task<IResult> SoftDeleteAsync(int id);
}