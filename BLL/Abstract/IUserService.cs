using DAL.EntityFramework.Utility;
using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<PaginatedList<UserResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize);

    Task<IDataResult<IEnumerable<UserResponseDto>>> GetAsync();

    Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id);

    Task<IResult> AddAsync(UserCreateRequestDto dto);

    Task<IResult> UpdateAsync(Guid id, UserUpdateRequestDto dto);

    Task<IResult> SoftDeleteAsync(Guid id);

    Task<IResult> SetImageAsync(Guid id, string? image = null);

    Task<IDataResult<string>> GetImageAsync(Guid id);

    Task<IResult> ResetPasswordAsync(Guid id, ResetPasswordRequestDto dto);
}