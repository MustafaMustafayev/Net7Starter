using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class RequestLog : IEntity
{
    [Key] public int RequestLogId { get; set; }

    public string? TraceIdentifier { get; set; }

    public string? ClientIp { get; set; }

    public string? Uri { get; set; }

    public DateTimeOffset RequestDate { get; set; }

    public string? Payload { get; set; }

    public string? Method { get; set; }

    public string? Token { get; set; }

    public int? UserId { get; set; }

    public virtual required ResponseLog ResponseLog { get; set; }

    [ForeignKey("ResponseLog")] public int ResponseLogId { get; set; }

    public bool IsDeleted { get; set; }
}