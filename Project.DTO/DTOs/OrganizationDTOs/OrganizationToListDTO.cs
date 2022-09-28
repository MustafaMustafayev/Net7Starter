namespace Project.DTO.DTOs.OrganizationDTOs;

public record OrganizationToListDto
{
    public int OrganizationId { get; set; }

    public string FullName { get; set; }

    public string ShortName { get; set; }

    public string Address { get; set; }

    public OrganizationToListDto ParentOrganization { get; set; }

    public int? ParentOrganizationId { get; set; }

    public string PhoneNumber { get; set; }

    public string Tin { get; set; }

    public string Email { get; set; }

    public string Rekvizit { get; set; }
}