namespace DTO.Responses;

public interface IDataResult<out T> : IResult
{
    T? Data { get; }
}