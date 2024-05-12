using DTO.User;

namespace DTO.Auth;

public record LoginResponseDto()
{

    public required UserResponseDto User { get; set; }
    public required string AccessToken { get; set; }
    public required DateTime AccessTokenExpireDate { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExpireDate { get; set; }
};