using Project.DTO.DTOs.UserDTOs;

namespace Project.DTO.DTOs.AuthDTOs;

public record LoginResponseDTO
{
    public UserToListDTO User { get; set; }

    public string Token { get; set; }
}