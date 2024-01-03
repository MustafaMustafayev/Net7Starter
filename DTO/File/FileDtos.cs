using ENTITIES.Enums;
using Microsoft.AspNetCore.Http;

namespace DTO.File;

public record FileToListDto
(
    Guid Id,
    string OriginalName,
    string HashName,
    string Extension,
    double Length,
    string? Path,
    FileType Type
);

public record FileToAddDto
(
    string OriginalName,
    string HashName,
    string Extension,
    double Length,
    string? Path,
    FileType Type
);

//add validator for this class
public record FileUploadRequestDto
(
    IFormFile File,
    FileType Type,
    Guid? UserId,
    Guid? OrganizationId
);

public record FileRemoveRequestDto
(
    string HashName,
    FileType Type,
    Guid? UserId,
    Guid? OrganizationId
);