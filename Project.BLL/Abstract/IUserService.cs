using Project.DAL.Utility;
using Project.DTO.Responses;
using Project.DTO.User;

namespace Project.BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<List<UserToListDto>>> GetAsync();
    Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize);

    Task<IDataResult<UserToListDto>> GetAsync(int userId);

    Task<IResult> AddAsync(UserToAddDto userToAddDto);

    Task<IResult> UpdateAsync(UserToUpdateDto userToUpdateDto);

    Task<IResult> DeleteAsync(int userId);
    Task<IResult> UpdateProfilePhotoAsync(int userId, string photoFileName);
    Task<IResult> DeleteProfilePhotoAsync(int userId);
}