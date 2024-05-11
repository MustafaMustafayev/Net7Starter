namespace DTO.Person;

public record PersonResponseDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Age { get; set; }
}