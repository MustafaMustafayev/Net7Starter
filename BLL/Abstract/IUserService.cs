using DAL.Utility;
using DTO.Responses;
using DTO.User;

namespace BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<IQueryable<UserToListDto>>> GetAsync();
    Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize);

    Task<IDataResult<UserToListDto>> GetAsync(int userId);

    Task<IResult> AddAsync(UserToAddDto userToAddDto);

    Task<IResult> UpdateAsync(UserToUpdateDto userToUpdateDto);

    Task<IResult> DeleteAsync(int userId);
    Task<IResult> UpdateProfilePhotoAsync(int userId, string photoFileName);
    Task<IResult> DeleteProfilePhotoAsync(int userId);
}