using DTO.Auth;
using DTO.Responses;
using DTO.Token;
using DTO.User;

namespace BLL.Abstract;

public interface ITokenService
{
    Task<IResult> AddAsync(LoginResponseDto dto);
    Task<IResult> SoftDeleteAsync(Guid id);
    Task<IDataResult<TokenToListDto>> GetAsync(string accessToken, string refreshToken);
    Task<IResult> CheckValidationAsync(string accessToken, string refreshToken);
    Task<IDataResult<LoginResponseDto>> CreateTokenAsync(UserToListDto dto);
}