using AutoMapper;
using BLL.Abstract;
using CORE.Enums;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.File;
using DTO.Responses;
using File = ENTITIES.Entities.File;

namespace BLL.Concrete;

public class FileService(IMapper mapper,
                         IUnitOfWork unitOfWork,
                         IUserService userService) : IFileService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserService _userService = userService;

    public async Task<IResult> AddFileAsync(FileCreateRequestDto dto, FileUploadRequestDto requestDto)
    {
        await AddAsync(dto);

        switch (dto.Type)
        {
            //case FileType.UserProfile:
            //    await _userService.AddProfileAsync(requestDto.UserId!.Value, fileId.Data);
            //    break;
            case EFileType.OrganizationLogo:
                // because of organization services are in mediatr
                // we don't need to inject this to here and add mediatr package to BLL just for this line.
                // that is example line and shows logic
                //await _organizationService.AddLogoAsync(requestDto.OrganizationId!.Value, fileId.Data);
                break;
        }

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> RemoveFileAsync(FileDeleteRequestDto dto)
    {
        await SoftDeleteAsync(dto.HashName);

        switch (dto.Type)
        {
            case EFileType.UserImages:
                await _userService.SetImageAsync(dto.UserId!.Value, null);
                break;
            case EFileType.OrganizationLogo:
                //await _organizationService.AddProfileAsync(userId!.Value, null);
                break;
        }

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<FileResponseDto>> GetAsync(string hashName)
    {
        var data = _mapper.Map<FileResponseDto>(await _unitOfWork.FileRepository.GetAsync(m => m.HashName == hashName));

        return new SuccessDataResult<FileResponseDto>(data, EMessages.Success.Translate());
    }

    private async Task<IDataResult<Guid>> AddAsync(FileCreateRequestDto dto)
    {
        var data = _mapper.Map<File>(dto);

        var added = await _unitOfWork.FileRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessDataResult<Guid>(added.Id, EMessages.Success.Translate());
    }

    private async Task<IResult> SoftDeleteAsync(string hashName)
    {
        var data = await _unitOfWork.FileRepository.GetAsync(m => m.HashName == hashName);

        _unitOfWork.FileRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }
}