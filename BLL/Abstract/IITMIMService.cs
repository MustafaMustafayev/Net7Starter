
using DTO.ITMIM;
using DTO.Responses;
using DAL.Utility;

namespace BLL.Abstract;

public interface IITMIMService
{
    Task<IDataResult<PaginatedList<ITMIMToListDto>>> GetAsPaginatedListAsync();
    Task<IDataResult<List<ITMIMToListDto>>> GetAsync();
    Task<IDataResult<ITMIMToListDto>> GetAsync(int id);
    Task<IResult> AddAsync(ITMIMToAddDto dto);
    Task<IResult> UpdateAsync(int id, ITMIMToUpdateDto dto);
    Task<IResult> SoftDeleteAsync(int id);
}
