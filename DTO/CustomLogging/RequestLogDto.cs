namespace DTO.CustomLogging;

public record RequestLogDto
{
    public string TraceIdentifier { get; set; }

    public string ClientIp { get; set; }

    public string Uri { get; set; }

    public DateTimeOffset RequestDate { get; set; }

    public string Payload { get; set; }

    public string Method { get; set; }

    public string Token { get; set; }

    public int? UserId { get; set; }

    public ResponseLogDto ResponseLog { get; set; }
}