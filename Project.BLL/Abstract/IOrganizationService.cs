using Project.DTO.DTOs.OrganizationDTOs;
using Project.DTO.DTOs.Responses;

namespace Project.BLL.Abstract
{
	public interface IOrganizationService
    {
        Task<IDataResult<List<OrganizationToListDTO>>> GetAsync();

        Task<IDataResult<OrganizationToListDTO>> GetAsync(int id);

        Task<IDataResult<Result>> AddAsync(OrganizationToAddOrUpdateDTO dto);

        Task<IDataResult<Result>> UpdateAsync(int id, OrganizationToAddOrUpdateDTO dto);

        Task DeleteAsync(int id);
    }
}