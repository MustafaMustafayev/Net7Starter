namespace DTO.Person;

public record PersonUpdateRequestDto
{
    public required string Name { get; set; }
    public required int Age { get; set; }
}