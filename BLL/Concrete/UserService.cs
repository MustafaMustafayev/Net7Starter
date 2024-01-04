using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Constants;
using CORE.Helpers;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DAL.EntityFramework.Utility;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using ENTITIES.Enums;

namespace BLL.Concrete;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IUtilService utilService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(UserToAddDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Email, null))
            return new ErrorResult(Messages.UserIsExist.Translate());

        dto = dto with
        {
            RoleId = !dto.RoleId.HasValue
                ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == UserType.Guest.ToString()))?.Id
                : dto.RoleId
        };
        var data = _mapper.Map<User>(dto);

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(data.Password, data.Salt);

        await _unitOfWork.UserRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.Id == id);

        _unitOfWork.UserRepository.SoftDelete(data!);

        var tokens = await _unitOfWork.TokenRepository.GetListAsync(m => m.UserId == id);
        tokens.ForEach(m => m.IsDeleted = true);

        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> AddProfileAsync(Guid userId, string? file)
    {
        var user = await _unitOfWork.UserRepository.GetAsNoTrackingAsync(u => u.Id == userId);
        user!.File = file;

        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.CommitAsync();

        return new SuccessResult();
    }

    public async Task<IDataResult<List<UserToListDto>>> GetAsync()
    {
        var datas = await _unitOfWork.UserRepository.GetListAsync();

        return new SuccessDataResult<List<UserToListDto>>(_mapper.Map<List<UserToListDto>>(datas),
            Messages.Success.Translate());
    }

    public async Task<IDataResult<UserToListDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<UserToListDto>(await _unitOfWork.UserRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<UserToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, UserToUpdateDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Email, id))
            return new ErrorResult(Messages.UserIsExist.Translate());

        dto = dto with
        {
            RoleId = dto.RoleId is null
                ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == UserType.Guest.ToString()))?.Id
                : dto.RoleId
        };

        var old = await _unitOfWork.UserRepository.GetAsNoTrackingAsync(u => u.Id == id);
        if (old is null) return new ErrorResult(Messages.UserIsNotExist.Translate());

        var data = _mapper.Map<User>(dto);

        data.Id = id;
        data.File = old.File;

        await _unitOfWork.UserRepository.UpdateUserAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.UserRepository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<User>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex,
            paginationDto.PageSize);

        var responseDto = new PaginatedList<UserToListDto>(
            _mapper.Map<List<UserToListDto>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);

        return new SuccessDataResult<PaginatedList<UserToListDto>>(responseDto,
            Messages.Success.Translate());
    }

    public async Task<IDataResult<string>> GetProfileAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.GetAsNoTrackingAsync(u => u.Id == userId);


        return new SuccessDataResult<string>(user.File, Messages.Success.Translate());
    }

    public async Task<IResult> UploadFileAsync(Guid id, Microsoft.AspNetCore.Http.IFormFile file)
    {
        User user = await _unitOfWork.UserRepository.GetAsync(m => m.Id == id);

        if (user is null)
        {
            return new ErrorDataResult<string>(Messages.UserIsNotExist.Translate());
        }


        string fileName = System.IO.Path.GetFileName(file.FileName);
        string fileExtension = System.IO.Path.GetExtension(file.FileName);
        Guid fileNewName = Guid.NewGuid();

        if (!Constants.AllowedImageExtensions.Contains(fileExtension))
            return new ErrorDataResult<string>(Messages.ThisFileTypeIsNotAllowed.Translate());

        var path = _utilService.GetEnvFolderPath(_utilService.GetFolderName(FileType.UserProfile));
        await FileHelper.WriteFile(file, $"{fileNewName}{fileExtension}", path);

        if (user!.File is not null)
        {
            var fullPath = System.IO.Path.Combine(path, user!.File);
           
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        user.File = $"{fileNewName}{fileExtension}";
        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> DeleteFileAsync(Guid id)
    {
        User user = await _unitOfWork.UserRepository.GetAsync(m => m.Id == id);

        if (user is null)
        {
            return new ErrorDataResult<string>(Messages.UserIsNotExist.Translate());
        }

        if (user!.File is null)
        {
            return new SuccessResult(Messages.Success.Translate());
        }

        var path = _utilService.GetEnvFolderPath(_utilService.GetFolderName(FileType.UserProfile));
        var fullPath = System.IO.Path.Combine(path, user!.File);

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }

        user.File = null;
        await _unitOfWork.UserRepository.UpdateUserAsync(user);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}