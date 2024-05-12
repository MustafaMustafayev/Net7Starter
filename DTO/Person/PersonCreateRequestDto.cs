namespace DTO.Person;

public record PersonCreateRequestDto
{
    public required string Name { get; set; }
    public required int Age { get; set; }
}