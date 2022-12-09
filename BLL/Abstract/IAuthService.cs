using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Abstract;

public interface IAuthService
{
    Task<string?> GetUserSaltAsync(string userEmail);
    Task<IDataResult<UserToListDto>> LoginAsync(LoginDto dto);
    Task<IDataResult<UserToListDto>> LoginByTokenAsync(string token);
    IResult SendVerificationCodeToEmailAsync(string email);
    Task<IResult> ResetPasswordAsync(ResetPasswordDto dto);
    Task<IResult> LogoutAsync(string accessToken);
}