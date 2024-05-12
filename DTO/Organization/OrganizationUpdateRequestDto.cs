namespace DTO.Organization;

public record OrganizationUpdateRequestDto
{
    public required string FullName { get; set; }
    public required string ShortName { get; set; }
    public required string Address { get; set; }
    public Guid? ParentId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Tin { get; set; }
    public string? Email { get; set; }
    public string? Rekvizit { get; set; }
}