
namespace Project.DTO.DTOs.AuthDTOs;

public record LoginDTO
{
    public string PIN { get; set; }

    public string Password { get; set; }
}