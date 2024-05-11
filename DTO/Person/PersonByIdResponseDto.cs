namespace DTO.Person;

public record PersonByIdResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
}