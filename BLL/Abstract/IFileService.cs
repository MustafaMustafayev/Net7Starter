using DTO.File;
using DTO.Responses;

namespace BLL.Abstract;

public interface IFileService
{
    Task<IDataResult<FileToListDto>> GetAsync(string hashName);

    Task<IDataResult<int>> AddAsync(FileToAddDto dto);

    Task<IResult> SoftDeleteAsync(string hashName);
}