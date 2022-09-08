namespace Project.DTO.DTOs.Responses;

public record SuccessResult : Result
{
    public SuccessResult(string message)
        : base(true, message)
    {
    }

    public SuccessResult()
        : base(true)
    {
    }
}