using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.RoleDTOs;

namespace Project.BLL.Abstract
{
	public interface IRoleService
    {
        Task<IDataResult<List<RoleToListDTO>>> GetAsync();

        Task<IDataResult<RoleToListDTO>> GetAsync(int id);

        Task<IDataResult<Result>> AddAsync(RoleToAddOrUpdateDTO dto);

        Task<IDataResult<Result>> UpdateAsync(int id, RoleToAddOrUpdateDTO dto);

        Task DeleteAsync(int id);
    }
}

