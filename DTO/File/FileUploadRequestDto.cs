using CORE.Enums;
using Microsoft.AspNetCore.Http;

namespace DTO.File;

//add validator for this class
public record FileUploadRequestDto
{
    public required IFormFile File { get; set; }
    public required EFileType Type { get; set; }
    public Guid? UserId { get; set; }
    public Guid? OrganizationId { get; set; }
}