namespace ENTITIES.Entities;

public class NLog : IEntity
{
    public int NLogId { get; set; }

    public string Application { get; set; }

    public DateTime LogDate { get; set; }

    public string Level { get; set; }

    public string Message { get; set; }

    public string Callsite { get; set; }

    public string Logger { get; set; }

    public string Exception { get; set; }
}