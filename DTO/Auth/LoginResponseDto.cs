using DTO.User;

namespace DTO.Auth;

public record LoginResponseDto
{
    public UserToListDto User { get; set; }

    public string Token { get; set; }
    public DateTime TokenExpireDate { get; set; }
}