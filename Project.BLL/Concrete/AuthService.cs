using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Abstract;
using Project.Core.CustomMiddlewares.Translation;
using Project.Core.Helper;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.Auth;
using Project.DTO.Responses;
using Project.DTO.User;

namespace Project.BLL.Concrete;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IUtilService utilService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _utilService = utilService;
    }

    public async Task<string?> GetUserSaltAsync(string userEmail)
    {
        return await _unitOfWork.UserRepository.GetUserSaltAsync(userEmail);
    }

    public async Task<IDataResult<UserToListDto>> LoginAsync(LoginDto loginDto)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m =>
            m.Email == loginDto.Email && m.Password == loginDto.Password);
        if (user == null)
            return new ErrorDataResult<UserToListDto>(Localization.Translate(Messages.InvalidUserCredentials));

        return new SuccessDataResult<UserToListDto>(_mapper.Map<UserToListDto>(user));
    }

    public async Task<IDataResult<UserToListDto>> LoginByTokenAsync(string token)
    {
        var userId = _utilService.GetUserIdFromToken(token);
        if (userId is null)
            return new ErrorDataResult<UserToListDto>(
                Localization.Translate(Messages.CanNotFoundUserIdInYourAccessToken));

        var user = await _unitOfWork.UserRepository.GetAsync(m => m.Id == userId);
        if (user == null)
            return new ErrorDataResult<UserToListDto>(Localization.Translate(Messages.InvalidUserCredentials));

        return new SuccessDataResult<UserToListDto>(_mapper.Map<UserToListDto>(user));
    }

    public IResult SendVerificationCodeToEmailAsync(string email)
    {
        //TODO SEND MAIL TO EMAIL
        return new SuccessResult(Localization.Translate(Messages.VerificationCodeSent));
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(m => m.Email == resetPasswordDto.Email);

        if (user is null) return new ErrorResult(Localization.Translate(Messages.UserIsNotExist));

        if (user.LastVerificationCode is null || !user.LastVerificationCode.Equals(resetPasswordDto.VerificationCode))
            return new ErrorResult(Localization.Translate(Messages.InvalidVerificationCode));

        user.Salt = SecurityHelper.GenerateSalt();
        user.Password = SecurityHelper.HashPassword(resetPasswordDto.Password, user.Salt);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Localization.Translate(Messages.PasswordResetted));
    }
}