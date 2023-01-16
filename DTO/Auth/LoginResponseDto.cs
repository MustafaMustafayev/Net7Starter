namespace DTO.Auth;

public record LoginResponseDto
{
    public required UserResponseDto User { get; set; }
    public required string AccessToken { get; set; }
    public DateTime AccessTokenExpireDate { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime RefreshTokenExpireDate { get; set; }
}