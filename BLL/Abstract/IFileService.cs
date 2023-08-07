using DTO.File;
using DTO.Responses;

namespace BLL.Abstract;

public interface IFileService
{
    Task<IDataResult<FileToListDto>> GetAsync(string hashName);
    Task<IResult> AddFileAsync(FileToAddDto dto, FileUploadRequestDto requestDto);
    Task<IResult> RemoveFileAsync(FileRemoveRequestDto dto);
}