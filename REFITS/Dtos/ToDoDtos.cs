namespace REFITS.Dtos;

public record ToDoToListDto
{
    public int ToDoId { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
}

public record ToDoToAddDto
{
    public string Title { get; set; }
    public bool Completed { get; set; }
}

public record ToDoToUpdateDto
{
    public int ToDoId { get; set; }
    public string Title { get; set; }
    public bool Completed { get; set; }
}