using Project.DTO.DTOs.UserDto;

namespace Project.DTO.DTOs.AuthDto;

public record LoginResponseDto
{
    public UserToListDto User { get; set; }

    public string Token { get; set; }
    public DateTime TokenExpireDate { get; set; }
}