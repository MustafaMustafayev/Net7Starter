namespace DTO.Person;

public record PersonToListDto
{
	public Guid Id { get; set; }
	public string Name {get; set;}
	public int Age {get; set;}

}
