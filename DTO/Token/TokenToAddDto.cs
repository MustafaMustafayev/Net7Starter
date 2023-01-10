namespace DTO.Token;

public record TokenToAddDto
{
    public int UserId { get; set; }
    public required string AccessToken { get; set; }
    public DateTimeOffset AccessTokenExpireDate { get; set; }
    public required string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpireDate { get; set; }
}