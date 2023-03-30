using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.File;
using DTO.Responses;
using File = ENTITIES.Entities.File;

namespace BLL.Concrete;

public class FileService : IFileService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public FileService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<int>> AddAsync(FileToAddDto dto)
    {
        var data = _mapper.Map<File>(dto);

        var added = await _unitOfWork.FileRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessDataResult<int>(added.FileId, Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(string hashName)
    {
        var data = await _unitOfWork.FileRepository.GetAsync(m => m.HashName == hashName);

        _unitOfWork.FileRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<FileToListDto>> GetAsync(string hashName)
    {
        var data = _mapper.Map<FileToListDto>(await _unitOfWork.FileRepository.GetAsync(m => m.HashName == hashName));

        return new SuccessDataResult<FileToListDto>(data, Messages.Success.Translate());
    }
}