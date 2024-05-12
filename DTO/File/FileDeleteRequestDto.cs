using CORE.Enums;

namespace DTO.File;

public record FileDeleteRequestDto
{
    public required string HashName { get; set; }
    public required EFileType Type { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OrganizationId { get; set; }
}