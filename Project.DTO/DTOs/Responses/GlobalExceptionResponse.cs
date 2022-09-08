namespace Project.DTO.DTOs.Responses;

public record GlobalExceptionResponse
{
    public bool Success { get; set; }

    public string Message { get; set; }
}