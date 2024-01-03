using DAL.EntityFramework.Utility;
using DTO.Responses;
using DTO.User;

namespace BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<List<UserToListDto>>> GetAsync();

    Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync();

    Task<IDataResult<UserToListDto>> GetAsync(Guid id);

    Task<IResult> AddAsync(UserToAddDto dto);

    Task<IResult> UpdateAsync(Guid id, UserToUpdateDto dto);

    Task<IResult> SoftDeleteAsync(Guid id);
    Task<IResult> AddProfileAsync(Guid userId, Guid? fileId);
}