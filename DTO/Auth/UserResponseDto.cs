using DTO.Role;

namespace DTO.Auth
{
    public record UserResponseDto
    {
        public required int UserId { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string ContactNumber { get; set; }
        public RoleToListDto? Role { get; set; }
        public string? ImagePath { get; set; }
    }
}
