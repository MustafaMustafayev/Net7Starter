namespace DTO.Token;

public record RefreshTokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}