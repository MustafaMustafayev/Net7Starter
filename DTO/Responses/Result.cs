using System.Text.Json.Serialization;

namespace DTO.Responses;

public record Result : IResult
{
    protected Result(bool success, string message)
        : this(success)
    {
        Message = message;
    }

    protected Result(bool success)
    {
        Success = success;
    }

    [JsonPropertyName("success")] public bool Success { get; set; }

    [JsonPropertyName("message")] public string? Message { get; set; }
}