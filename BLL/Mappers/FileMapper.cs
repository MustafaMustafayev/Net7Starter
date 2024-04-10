using AutoMapper;
using DTO.File;
using File = ENTITIES.Entities.File;

namespace BLL.Mappers;

public class FileMapper : Profile
{
    public FileMapper()
    {
        CreateMap<FileCreateRequestDto, File>();
        CreateMap<File, FileResponseDto>();
    }
}