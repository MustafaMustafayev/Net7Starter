using DAL.EntityFramework.Utility;
using DTO.Responses;
using DTO.User;

namespace BLL.Abstract;

public interface IUserService
{
    Task<IDataResult<IEnumerable<UserResponseDto>>> GetAsync();

    Task<IDataResult<PaginatedList<UserResponseDto>>> GetAsPaginatedListAsync();

    Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id);

    Task<IResult> AddAsync(UserCreateRequestDto dto);

    Task<IResult> UpdateAsync(Guid id, UserUpdateRequestDto dto);

    Task<IResult> SoftDeleteAsync(Guid id);
    Task<IResult> AddProfileAsync(Guid userId, string? file = null);
    Task<IDataResult<string>> GetProfileAsync(Guid userId);

}