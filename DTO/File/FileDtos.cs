using ENTITIES.Enums;
using Microsoft.AspNetCore.Http;

namespace DTO.File;

public record FileToListDto
(
    int Id,
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
    int? UserId,
    int? OrganizationId
);

public record FileRemoveRequestDto
(
    string HashName,
    FileType Type,
    int? UserId,
    int? OrganizationId
);