using DTO.File;
using DTO.Responses;

namespace BLL.Abstract;

public interface IFileService
{
    Task<IDataResult<FileResponseDto>> GetAsync(string hashName);
    Task<IResult> AddFileAsync(FileCreateRequestDto dto, FileUploadRequestDto requestDto);
    Task<IResult> RemoveFileAsync(FileDeleteRequestDto dto);
}