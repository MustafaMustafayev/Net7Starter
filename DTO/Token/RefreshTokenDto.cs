namespace DTO.Token;

public record RefreshTokenDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}