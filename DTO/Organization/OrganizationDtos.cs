using DTO.File;

namespace DTO.Organization;

public record OrganizationToAddDto
(
    string FullName,
    string ShortName,
    string Address,
    int? ParentId,
    string PhoneNumber,
    string Tin,
    string Email,
    string Rekvizit
);

public record OrganizationToListDto
(
    int Id,
    string FullName,
    string ShortName,
    string Address,
    OrganizationToListDto? Parent,
    string PhoneNumber,
    string Tin,
    string Email,
    string Rekvizit,
    FileToListDto? LogoFile
);

public record OrganizationToUpdateDto
(
    string FullName,
    string ShortName,
    string Address,
    int? ParentId,
    string PhoneNumber,
    string Tin,
    string Email,
    string Rekvizit
);