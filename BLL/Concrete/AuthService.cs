using AutoMapper;
using BLL.Abstract;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Concrete;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;

    public AuthService(IUnitOfWork unitOfWork,
                       IMapper mapper,
                       IUtilService utilService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _utilService = utilService;
    }

    public async Task<string?> GetUserSaltAsync(string userEmail)
    {
        return await _unitOfWork.UserRepository.GetUserSaltAsync(userEmail);
    }

    public async Task<IDataResult<UserResponseDto>> LoginAsync(LoginRequestDto dtos)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.Email == dtos.Email && m.Password == dtos.Password);
        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserResponseDto>> LoginByTokenAsync()
    {
        var userId = _utilService.GetUserIdFromToken();
        if (userId is null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.CanNotFoundUserIdInYourAccessToken.Translate());
        }

        var data = await _unitOfWork.UserRepository.GetAsync(m => m.Id == userId);
        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public IResult SendVerificationCodeToEmailAsync(string email)
    {
        //TODO SEND MAIL TO EMAIL
        return new SuccessResult(EMessages.VerificationCodeSent.Translate());
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequestDto dto)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.Email == dto.Email);

        if (data is null)
        {
            return new ErrorResult(EMessages.UserIsNotExist.Translate());
        }

        if (data.LastVerificationCode is null || !data.LastVerificationCode.Equals(dto.VerificationCode))
        {
            return new ErrorResult(EMessages.InvalidVerificationCode.Translate());
        }

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(dto.Password, data.Salt);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.PasswordResetted.Translate());
    }

    public async Task<IResult> LogoutAsync(string accessToken)
    {
        var tokens = await _unitOfWork.TokenRepository.GetActiveTokensAsync(accessToken);

        tokens.ForEach(m => m.IsDeleted = true);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> LogoutRemovedUserAsync(Guid userId)
    {
        var tokens = (await _unitOfWork.TokenRepository.GetListAsync(m => m.UserId == userId)).ToList();
        tokens.ForEach(m => m.IsDeleted = true);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }
}