namespace DTO.Department;

public record DepartmentUpdateRequestDto()
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}