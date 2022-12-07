using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Config;
using CORE.Helper;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Auth;
using DTO.Responses;
using DTO.Token;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class TokenService : ITokenService
{
    private readonly ConfigSettings _configSettings;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;

    public TokenService(ConfigSettings configSettings, IUnitOfWork unitOfWork, IUtilService utilService, IMapper mapper)
    {
        _configSettings = configSettings;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(LoginResponseDto dto)
    {
        var data = _mapper.Map<Token>(dto);

        await _unitOfWork.TokenRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<TokenToListDto>> GetAsync(RefreshTokenDto dto)
    {
        var data = _mapper.Map<TokenToListDto>(
            (await _unitOfWork.TokenRepository.GetAsNoTrackingAsync(m =>
                m.AccessToken == dto.AccessToken && m.RefreshToken == dto.RefreshToken &&
                m.RefreshTokenExpireDate > DateTime.UtcNow))!);

        return new SuccessDataResult<TokenToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> CheckValidationAsync(string accessToken, string refreshToken)
    {
        return await _unitOfWork.TokenRepository.IsValid(accessToken, refreshToken)
            ? new SuccessResult(Messages.Success.Translate())
            : new ErrorResult(Messages.PermissionDenied.Translate());
    }

    public async Task<IDataResult<LoginResponseDto>> CreateTokenAsync(UserToListDto dto)
    {
        var securityHelper = new SecurityHelper(_configSettings);
        var accessTokenExpireDate =
            DateTime.UtcNow.AddHours(_configSettings.AuthSettings.TokenExpirationTimeInHours);

        var loginResponseDto = new LoginResponseDto
        {
            User = dto,
            AccessToken = securityHelper.CreateTokenForUser(dto, accessTokenExpireDate),
            AccessTokenExpireDate = accessTokenExpireDate,
            RefreshToken = _utilService.GenerateRefreshToken(),
            RefreshTokenExpireDate =
                accessTokenExpireDate.AddMinutes(_configSettings.AuthSettings
                    .RefreshTokenAdditionalMinutes)
        };

        await AddAsync(loginResponseDto);

        return new SuccessDataResult<LoginResponseDto>(loginResponseDto, Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.TokenRepository.GetAsync(m => m.UserId == id);

        _unitOfWork.TokenRepository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}