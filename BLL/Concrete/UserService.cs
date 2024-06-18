using AutoMapper;
using BLL.Abstract;
using BLL.Helpers;
using CORE.Enums;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Utility;
using DTO.Auth;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class UserService(IMapper mapper,
                         IUserRepository userRepository,
                         IRoleRepository roleRepository,
                         ITokenRepository tokenRepository) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;

    public async Task<IResult> AddAsync(UserCreateRequestDto dto)
    {
        if (await _userRepository.IsUserExistAsync(dto.Email, null))
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        dto = dto with
        {
            RoleId = !dto.RoleId.HasValue
                     ? (await _roleRepository.GetAsync(m => m.Key == EUserType.Guest.ToString()))?.Id
                     : dto.RoleId
        };
        var data = _mapper.Map<User>(dto);

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(data.Password, data.Salt);

        await _userRepository.AddAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _userRepository.GetAsync(m => m.Id == id);

        await _userRepository.SoftDeleteAsync(data!);

        var tokens = (await _tokenRepository.GetListAsync(m => m.UserId == id)).ToList();
        tokens.ForEach(m => m.IsDeleted = true);
        await _tokenRepository.UpdateRangeAsync(tokens);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SetImageAsync(Guid id, string? image = null)
    {
        var user = await _userRepository.GetAsync(u => u.Id == id);
        user!.Image = image;

        await _userRepository.UpdateAsync(user);
        return new SuccessResult();
    }

    public async Task<IDataResult<IEnumerable<UserResponseDto>>> GetAsync()
    {
        var datas = await _userRepository.GetListAsync();
        return new SuccessDataResult<IEnumerable<UserResponseDto>>(_mapper.Map<IEnumerable<UserResponseDto>>(datas), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserByIdResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<UserByIdResponseDto>(await _userRepository.GetAsync(m => m.Id == id));
        return new SuccessDataResult<UserByIdResponseDto>(data, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, UserUpdateRequestDto dto)
    {
        if (await _userRepository.IsUserExistAsync(dto.Email, id))
        {
            return new ErrorResult(EMessages.UserIsExist.Translate());
        }

        dto = dto with
        {
            RoleId = dto.RoleId is null
                     ? (await _roleRepository.GetAsync(m => m.Key == EUserType.Guest.ToString()))?.Id
                     : dto.RoleId
        };

        var data = _mapper.Map<User>(dto);
        data.Id = id;

        await _userRepository.UpdateUserAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<UserResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _userRepository.GetList();

        var response = await PaginatedList<User>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<UserResponseDto>(_mapper.Map<List<UserResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<UserResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IDataResult<string>> GetImageAsync(Guid id)
    {
        var user = await _userRepository.GetAsNoTrackingAsync(u => u.Id == id);
        if (user is { Image: not null })
        {
            return new SuccessDataResult<string>(user.Image, EMessages.Success.Translate());
        }
        return new SuccessDataResult<string>(EMessages.FileIsNotFound.Translate());
    }

    public async Task<IResult> ResetPasswordAsync(Guid id, ResetPasswordRequestDto dto)
    {
        var data = await _userRepository.GetAsync(m => m.Id == id);

        if (data is null)
        {
            return new ErrorResult(EMessages.UserIsNotExist.Translate());
        }

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(dto.Password, data.Salt);

        await _userRepository.UpdateAsync(data);

        return new SuccessResult(EMessages.PasswordResetted.Translate());
    }
}