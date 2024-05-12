namespace DTO.Department;

public record DepartmentCreateRequestDto()
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}