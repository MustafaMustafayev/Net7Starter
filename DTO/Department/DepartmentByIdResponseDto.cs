namespace DTO.Department;
public record DepartmentByIdResponseDto()
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}