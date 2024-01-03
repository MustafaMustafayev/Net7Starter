using DTO.Permission;
using DTO.Responses;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Abstract;

public interface IRoleService
{
    Task<IDataResult<List<RoleToListDto>>> GetAsync();

    Task<IDataResult<List<PermissionToListDto>>> GetPermissionsAsync(Guid id);

    Task<IDataResult<IQueryable<Role>>> GraphQlGetAsync();

    Task<IDataResult<RoleToListDto>> GetAsync(Guid id);

    Task<IResult> AddAsync(RoleToAddDto dto);

    Task<IResult> UpdateAsync(Guid id, RoleToUpdateDto dto);

    Task<IResult> SoftDeleteAsync(Guid id);
}