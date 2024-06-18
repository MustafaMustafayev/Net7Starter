using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Abstract;

public interface IAuthService
{
    Task<string?> GetUserSaltAsync(string userEmail);

    Task<IDataResult<UserResponseDto>> LoginAsync(LoginRequestDto dto);

    Task<IDataResult<UserResponseDto>> LoginByTokenAsync();

    Task<IResult> LogoutAsync(string accessToken);

    Task<IResult> LogoutRemovedUserAsync(Guid userId);
}