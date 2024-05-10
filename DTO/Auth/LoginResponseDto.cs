using DTO.User;

namespace DTO.Auth;

public record LoginResponseDto()
{

    public UserResponseDto User { get; set; }
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpireDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpireDate { get; set; }
};
