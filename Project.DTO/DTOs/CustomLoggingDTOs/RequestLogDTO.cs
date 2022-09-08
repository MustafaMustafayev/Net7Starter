namespace Project.DTO.DTOs.CustomLoggingDTOs;

public record RequestLogDTO
{
    public string TraceIdentifier { get; set; }

    public string ClientIP { get; set; }

    public string URI { get; set; }

    public DateTimeOffset RequestDate { get; set; }

    public string Payload { get; set; }

    public string Method { get; set; }

    public string Token { get; set; }

    public int? UserId { get; set; }

    public ResponseLogDTO ResponseLog { get; set; }
}