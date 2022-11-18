namespace DTO.Responses;

public interface IDataResult<T> : IResult
{
    T Data { get; }
}