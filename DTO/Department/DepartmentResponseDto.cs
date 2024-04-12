namespace DTO.Department;
public record DepartmentResponseDto()
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}