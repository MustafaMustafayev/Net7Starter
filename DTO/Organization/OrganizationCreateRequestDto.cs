namespace DTO.Organization;

public record OrganizationCreateRequestDto
{
    public required string FullName { get; set; }
    public required string ShortName { get; set; }
    public string? Address { get; set; }
    public Guid? ParentId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Tin { get; set; }
    public string? Email { get; set; }
    public string? Rekvizit { get; set; }
}