namespace Project.DTO.Responses;

public interface IResult
{
    bool Success { get; }

    string Message { get; }
}