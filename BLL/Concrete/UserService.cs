using AutoMapper;
using BLL.Abstract;
using CORE.Helper;
using CORE.Middlewares.Translation;
using DAL.UnitOfWorks.Abstract;
using DAL.Utility;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> AddAsync(UserToAddDto userToAddDto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(userToAddDto.Username, null))
            return new ErrorResult(Localization.Translate(Messages.UserIsExist));

        var user = _mapper.Map<User>(userToAddDto);

        user.Salt = SecurityHelper.GenerateSalt();
        user.Password = SecurityHelper.HashPassword(user.Password, user.Salt);

        var added = await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Localization.Translate(Messages.Success));
    }

    public async Task<IResult> DeleteAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == userId);
        user!.IsDeleted = true;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return new SuccessResult();
    }

    public Task<IDataResult<List<UserToListDto>>> GetAsync()
    {
        var users = _unitOfWork.UserRepository.GetAsNoTrackingList().ToList();

        return Task.FromResult<IDataResult<List<UserToListDto>>>(
            new SuccessDataResult<List<UserToListDto>>(_mapper.Map<List<UserToListDto>>(users)));
    }

    public async Task<IDataResult<UserToListDto>> GetAsync(int userId)
    {
        var user = _mapper.Map<UserToListDto>(
            (await _unitOfWork.UserRepository.GetAsNoTrackingAsync(m => m.UserId == userId))!);

        return new SuccessDataResult<UserToListDto>(user);
    }

    public async Task<IResult> UpdateAsync(UserToUpdateDto userToUpdateDto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(userToUpdateDto.Username, userToUpdateDto.UserId))
            return new ErrorResult(Localization.Translate(Messages.UserIsExist));

        await _unitOfWork.UserRepository.UpdateUserAsync(_mapper.Map<User>(userToUpdateDto));
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Localization.Translate(Messages.Success));
    }

    public async Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var users = _unitOfWork.UserRepository.GetAsNoTrackingList();
        var response = await PaginatedList<User>.CreateAsync(users.OrderBy(m => m.UserId), pageIndex, pageSize);

        var responseDto = new PaginatedList<UserToListDto>(_mapper.Map<List<UserToListDto>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);

        return new SuccessDataResult<PaginatedList<UserToListDto>>(responseDto);
    }

    public async Task<IResult> UpdateProfilePhotoAsync(int userId, string photoFileName)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == userId);
        if (user == null) return new ErrorResult(Localization.Translate(Messages.InvalidUserCredentials));

        user.Photo.FileName = photoFileName;

        _unitOfWork.UserRepository.Update(_mapper.Map<User>(user));
        await _unitOfWork.CommitAsync();

        return new SuccessResult();
    }

    public async Task<IResult> DeleteProfilePhotoAsync(int userId)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == userId);
        if (user == null) return new ErrorResult(Localization.Translate(Messages.InvalidUserCredentials));

        user.Photo = null;

        _unitOfWork.UserRepository.Update(_mapper.Map<User>(user));
        await _unitOfWork.CommitAsync();

        return new SuccessResult();
    }
}