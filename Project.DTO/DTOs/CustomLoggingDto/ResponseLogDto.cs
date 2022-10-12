namespace Project.DTO.DTOs.CustomLoggingDto;

public record ResponseLogDto
{
    public string TraceIdentifier { get; set; }

    public DateTimeOffset ResponseDate { get; set; }

    public string StatusCode { get; set; }

    public string Token { get; set; }

    public int? UserId { get; set; }
}