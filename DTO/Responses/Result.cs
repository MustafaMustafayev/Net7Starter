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

    public bool Success { get; }

    public string? Message { get; }
}