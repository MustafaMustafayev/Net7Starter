using DTO.User;

namespace DTO.Token;

public record TokenToListDto()
{
    public required Guid Id { get; set; }
    public required UserResponseDto User { get; set; }
    public required string AccessToken { get; set; }
    public required DateTimeOffset AccessTokenExpireDate { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTimeOffset RefreshTokenExpireDate { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}