namespace DTO.Organization;

public record OrganizationToListDto
{
    public int OrganizationId { get; set; }

    public required string FullName { get; set; }

    public required string ShortName { get; set; }

    public required string Address { get; set; }

    public OrganizationToListDto ParentOrganization { get; set; }

    public int? ParentOrganizationId { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Tin { get; set; }

    public required string Email { get; set; }

    public required string Rekvizit { get; set; }
}