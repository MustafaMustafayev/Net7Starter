using DTO.File;

namespace DTO.Organization;

public record OrganizationToAddDto
(
    string FullName,
    string ShortName,
    string Address,
    Guid? ParentId,
    string PhoneNumber,
    string Tin,
    string Email,
    string Rekvizit
);

public record OrganizationToListDto
(
    Guid Id,
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
    Guid? ParentId,
    string PhoneNumber,
    string Tin,
    string Email,
    string Rekvizit
);