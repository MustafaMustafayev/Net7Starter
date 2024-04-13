using DTO.Permission;
using DTO.Responses;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Abstract;

public interface IRoleService
{
    Task<IDataResult<IEnumerable<RoleResponseDto>>> GetAsync();

    Task<IDataResult<IEnumerable<PermissionResponseDto>>> GetPermissionsAsync(Guid id);

    Task<IDataResult<IQueryable<Role>>> GraphQlGetAsync();

    Task<IDataResult<RoleByIdResponseDto>> GetAsync(Guid id);

    Task<IResult> AddAsync(RoleCreateRequestDto dto);

    Task<IResult> UpdateAsync(Guid id, RoleUpdateRequestDto dto);

    Task<IResult> SoftDeleteAsync(Guid id);
}