using AutoMapper;
using BLL.Abstract;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Config;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Auth;
using DTO.Responses;
using DTO.Token;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class TokenService(ConfigSettings configSettings,
                         IMapper mapper,
                         IUnitOfWork unitOfWork,
                         IUtilService utilService) : ITokenService
{
    private readonly ConfigSettings _configSettings = configSettings;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUtilService _utilService = utilService;

    public async Task<IResult> AddAsync(LoginResponseDto dto)
    {
        var data = _mapper.Map<Token>(dto);

        await _unitOfWork.TokenRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<TokenToListDto>> GetAsync(string accessToken, string refreshToken)
    {
        var token = await _unitOfWork.TokenRepository.GetAsync(m => m.AccessToken == accessToken &&
                                                                    m.RefreshToken == refreshToken &&
                                                                    m.RefreshTokenExpireDate > DateTime.UtcNow);

        if (token == null)
        {
            return new ErrorDataResult<TokenToListDto>(EMessages.PermissionDenied.Translate());
        }

        var data = _mapper.Map<TokenToListDto>(token);

        return new SuccessDataResult<TokenToListDto>(data, EMessages.Success.Translate());
    }

    public async Task<IResult> CheckValidationAsync(string accessToken, string refreshToken)
    {
        return await _unitOfWork.TokenRepository.IsValid(accessToken, refreshToken)
               ? new SuccessResult(EMessages.Success.Translate())
               : new ErrorResult(EMessages.PermissionDenied.Translate());
    }

    public async Task<IDataResult<LoginResponseDto>> CreateTokenAsync(UserResponseDto dto)
    {
        var securityHelper = new SecurityHelper(_configSettings, _utilService);
        var accessTokenExpireDate = DateTime.UtcNow.AddHours(_configSettings.AuthSettings.TokenExpirationTimeInHours);

        var loginResponseDto = new LoginResponseDto()
        {
            User = dto,
            AccessToken = securityHelper.CreateTokenForUser(dto, accessTokenExpireDate),
            AccessTokenExpireDate = accessTokenExpireDate,
            RefreshToken = _utilService.GenerateRefreshToken(),
            RefreshTokenExpireDate = accessTokenExpireDate.AddMinutes(_configSettings.AuthSettings.RefreshTokenAdditionalMinutes)
        };

        await AddAsync(loginResponseDto);

        return new SuccessDataResult<LoginResponseDto>(loginResponseDto, EMessages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.TokenRepository.GetAsync(m => m.Id == id);

        _unitOfWork.TokenRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }
}