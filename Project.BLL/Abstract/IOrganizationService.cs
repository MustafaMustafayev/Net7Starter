using Project.DTO.DTOs.OrganizationDTOs;
using Project.DTO.DTOs.Responses;

namespace Project.BLL.Abstract;

public interface IOrganizationService
{
    Task<IDataResult<List<OrganizationToListDto>>> GetAsync();

    Task<IDataResult<OrganizationToListDto>> GetAsync(int id);

    Task<IResult> AddAsync(OrganizationToAddOrUpdateDto dto);

    Task<IResult> UpdateAsync(OrganizationToAddOrUpdateDto dto);

    Task<IResult> DeleteAsync(int id);
}