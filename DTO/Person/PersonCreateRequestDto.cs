namespace DTO.Person;

public record PersonCreateRequestDto
{
    public string Name { get; set; }
    public int Age { get; set; }
}