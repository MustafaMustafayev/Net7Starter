namespace DTO.Logging;

public record RequestLogDto
{
    public string? TraceIdentifier { get; set; }
    public string? ClientIp { get; set; }
    public string? Uri { get; set; }
    public DateTimeOffset RequestDate { get; set; }
    public string? Payload { get; set; }
    public string? Method { get; set; }
    public string? Token { get; set; }
    public Guid? UserId { get; set; }
    public required ResponseLogDto ResponseLog { get; set; }
}