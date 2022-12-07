using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Auth;
using DTO.Responses;
using DTO.Token;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class TokenService : ITokenService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.TokenRepository.GetAsync(m => m.UserId == id);

        _unitOfWork.TokenRepository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}