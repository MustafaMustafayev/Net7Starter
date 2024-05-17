using AutoMapper;
using BLL.Abstract;
using BLL.Helpers;
using CORE.Enums;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DAL.EntityFramework.Utility;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class UserService(IMapper mapper,
                         IUnitOfWork unitOfWork) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IResult> AddAsync(UserCreateRequestDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Email, null))
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        dto = dto with
        {
            RoleId = !dto.RoleId.HasValue
                     ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == EUserType.Guest.ToString()))?.Id
                     : dto.RoleId
        };
        var data = _mapper.Map<User>(dto);

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(data.Password, data.Salt);

        await _unitOfWork.UserRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.Id == id);

        _unitOfWork.UserRepository.SoftDelete(data!);

        var tokens = (await _unitOfWork.TokenRepository.GetListAsync(m => m.UserId == id)).ToList();
        tokens.ForEach(m => m.IsDeleted = true);

        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SetImageAsync(Guid userId, string? image = null)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId);
        user!.Image = image;

        _unitOfWork.UserRepository.Update(user);
        await _unitOfWork.CommitAsync();

        return new SuccessResult();
    }

    public async Task<IDataResult<IEnumerable<UserResponseDto>>> GetAsync()
    {
        var datas = await _unitOfWork.UserRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<UserResponseDto>>(_mapper.Map<IEnumerable<UserResponseDto>>(datas), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<UserByIdResponseDto>(await _unitOfWork.UserRepository.GetAsync(m => m.Id == id));
        return new SuccessDataResult<UserByIdResponseDto>(data, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, UserUpdateRequestDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Email, id))
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        dto = dto with
        {
            RoleId = dto.RoleId is null
                     ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == EUserType.Guest.ToString()))?.Id
                     : dto.RoleId
        };

        var data = _mapper.Map<User>(dto);
        data.Id = id;

        await _unitOfWork.UserRepository.UpdateUserAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<UserResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _unitOfWork.UserRepository.GetList();

        var response = await PaginatedList<User>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<UserResponseDto>(_mapper.Map<List<UserResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<UserResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IDataResult<string>> GetImageAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.GetAsNoTrackingAsync(u => u.Id == userId);
        if (user is { Image: not null })
        {
            return new SuccessDataResult<string>(user.Image, EMessages.Success.Translate());
        }
        return new SuccessDataResult<string>(EMessages.FileIsNotFound.Translate());
    }
}