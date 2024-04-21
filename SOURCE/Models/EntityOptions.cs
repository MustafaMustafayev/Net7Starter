namespace SOURCE.Models;

public class EntityOptions
{
    public bool BuildRepository { get; set; } = true;
    public bool BuildService { get; set; } = true;
    public bool BuildUnitOfWork { get; set; } = true;
    public bool BuildController { get; set; } = true;
}
