using Project.DTO.Responses;
using Project.DTO.Role;

namespace Project.BLL.Abstract;

public interface IRoleService
{
    Task<IDataResult<List<RoleToListDto>>> GetAsync();

    Task<IDataResult<RoleToListDto>> GetAsync(int id);

    Task<IResult> AddAsync(RoleToAddDto dto);

    Task<IResult> UpdateAsync(RoleToUpdateDto dto);

    Task<IResult> DeleteAsync(int id);
}