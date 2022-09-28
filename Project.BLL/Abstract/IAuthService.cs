using Project.DTO.DTOs.AuthDto;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDto;

namespace Project.BLL.Abstract;

public interface IAuthService
{
    Task<string?> GetUserSaltAsync(string userEmail);
    Task<IDataResult<UserToListDto>> LoginAsync(LoginDto loginDto);
    Task<IDataResult<UserToListDto>> LoginByTokenAsync(string token);
    IResult SendVerificationCodeToEmailAsync(string email);
    Task<IResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}