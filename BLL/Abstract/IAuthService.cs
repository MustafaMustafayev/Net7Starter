using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Abstract;

public interface IAuthService
{
    Task<string?> GetUserSaltAsync(string userEmail);

    Task<IDataResult<UserResponseDto>> LoginAsync(LoginRequestDto dtos);

    Task<IDataResult<UserResponseDto>> LoginByTokenAsync();

    IResult SendVerificationCodeToEmailAsync(string email);

    Task<IResult> ResetPasswordAsync(ResetPasswordRequestDto dto);

    Task<IResult> LogoutAsync(string accessToken);

    Task<IResult> LogoutRemovedUserAsync(Guid userId);
}