using DTO.Responses;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Abstract;

public interface IRoleService
{
    Task<IDataResult<List<RoleToListDto>>> GetAsync();
    Task<IDataResult<IQueryable<Role>>> GraphQlGetAsync();

    Task<IDataResult<RoleToListDto>> GetAsync(int id);

    Task<IResult> AddAsync(RoleToAddDto dto);

    Task<IResult> UpdateAsync(RoleToUpdateDto dto);

    Task<IResult> DeleteAsync(int id);
}