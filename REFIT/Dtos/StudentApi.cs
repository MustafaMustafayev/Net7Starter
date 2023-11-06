namespace BLL.External.Dtos;

public class StudentToAddDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class StudentToUpdateDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
}

public class StudentToListDto
{
    public int StudentId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
}