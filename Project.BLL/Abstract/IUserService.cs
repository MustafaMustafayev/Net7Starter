using Project.DAL.Utility;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDto;

namespace Project.BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<List<UserToListDto>>> GetAsync();
    Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize);

    Task<IDataResult<UserToListDto>> GetAsync(int userId);

    Task<IResult> AddAsync(UserToAddDto userToAddDto);

    Task<IResult> UpdateAsync(UserToUpdateDto userToUpdateDto);

    Task<IResult> DeleteAsync(int userId);
}