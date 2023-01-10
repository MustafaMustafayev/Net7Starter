using DTO.User;

namespace DTO.Token;

public record TokenToListDto
{
    public int TokenId { get; set; }
    public required UserToListDto User { get; set; }
    public required string AccessToken { get; set; }
    public DateTimeOffset AccessTokenExpireDate { get; set; }
    public required string RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpireDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }
}