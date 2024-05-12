namespace DTO.Auth;

public record LoginRequestDto()
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}