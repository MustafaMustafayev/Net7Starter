using Project.DTO.Responses;
using Project.DTO.Role;

namespace Project.BLL.Abstract;

public interface IRoleService
{
    Task<IDataResult<List<RoleToListDto>>> GetAsync();

    Task<IDataResult<RoleToListDto>> GetAsync(int id);

    Task<IDataResult<Result>> AddAsync(RoleToAddOrUpdateDto dto);

    Task<IDataResult<Result>> UpdateAsync(RoleToAddOrUpdateDto dto);

    Task<IResult> DeleteAsync(int id);
}