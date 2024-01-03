using ENTITIES.Entities.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITIES.Entities;

public class RequestLog : IEntity
{
    [Key] public Guid RequestLogId { get; set; }

    public string? TraceIdentifier { get; set; }

    public string? ClientIp { get; set; }

    public string? Uri { get; set; }

    public DateTimeOffset RequestDate { get; set; }

    public string? Payload { get; set; }

    public string? Method { get; set; }

    public string? Token { get; set; }

    public Guid? UserId { get; set; }

    public virtual required ResponseLog ResponseLog { get; set; }

    [ForeignKey("ResponseLog")] public Guid ResponseLogId { get; set; }

    public bool IsDeleted { get; set; }
}