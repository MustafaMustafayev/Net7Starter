namespace DTO.Person;

public record PersonUpdateRequestDto
{
    public string Name { get; set; }
    public int Age { get; set; }

}
