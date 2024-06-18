using AutoMapper;
using BLL.Abstract;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace BLL.Concrete;

public class AuthService(IMapper mapper,
                         IUserRepository userRepository,
                         ITokenRepository tokenRepository,
                         IUtilService utilService) : IAuthService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    private readonly IUtilService _utilService = utilService;

    public async Task<string?> GetUserSaltAsync(string userEmail)
    {
        return await _userRepository.GetUserSaltAsync(userEmail);
    }

    public async Task<IDataResult<UserResponseDto>> LoginAsync(LoginRequestDto dto)
    {
        var data = await _userRepository.GetAsync(m => m.Email == dto.Email && m.Password == dto.Password);
        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public async Task<IDataResult<UserResponseDto>> LoginByTokenAsync()
    {
        var userId = _utilService.GetUserIdFromToken();

        var data = await _userRepository.GetAsync(m => m.Id == userId);
        if (data == null)
        {
            return new ErrorDataResult<UserResponseDto>(EMessages.InvalidUserCredentials.Translate());
        }

        return new SuccessDataResult<UserResponseDto>(_mapper.Map<UserResponseDto>(data), EMessages.Success.Translate());
    }

    public async Task<IResult> LogoutAsync(string accessToken)
    {
        var tokens = await _tokenRepository.GetActiveTokensAsync(accessToken);
        tokens.ForEach(m => m.IsDeleted = true);
        await _tokenRepository.UpdateRangeAsync(tokens);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> LogoutRemovedUserAsync(Guid userId)
    {
        var tokens = (await _tokenRepository.GetListAsync(m => m.UserId == userId)).ToList();
        tokens.ForEach(m => m.IsDeleted = true);
        await _tokenRepository.UpdateRangeAsync(tokens);

        return new SuccessResult(EMessages.Success.Translate());
    }
}