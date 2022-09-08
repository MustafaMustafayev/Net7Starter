namespace Project.DTO.DTOs.CustomLoggingDTOs;

public record ResponseLogDTO
{
    public string TraceIdentifier { get; set; }

    public DateTimeOffset ResponseDate { get; set; }

    public string StatusCode { get; set; }

    public string Token { get; set; }

    public int? UserId { get; set; }
}