namespace DTO.Organization;

public record OrganizationToAddDto(string FullName, string ShortName,
    string Address, int? ParentOrganizationId, string PhoneNumber,
    string Tin, string Email, string Rekvizit);

public record OrganizationToListDto(int OrganizationId, string FullName,
    string ShortName, string Address, OrganizationToListDto? ParentOrganization,
    int? ParentOrganizationId, string PhoneNumber, string Tin, string Email, string Rekvizit);

public record OrganizationToUpdateDto(string FullName, string ShortName, string Address,
    int? ParentOrganizationId, string PhoneNumber, string Tin, string Email, string Rekvizit);