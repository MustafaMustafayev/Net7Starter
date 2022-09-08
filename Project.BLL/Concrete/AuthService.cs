using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

public class AuthService : IAuthService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<string> GetUserSaltAsync(string pin)
    {
        return await _unitOfWork.UserRepository.GetUserSaltAsync(pin);
    }

    public async Task<IDataResult<UserToListDTO>> LoginAsync(LoginDTO loginDto)
    {
        User user = await _unitOfWork.UserRepository.GetAsync(m =>
            m.PIN == loginDto.PIN && m.Password == loginDto.Password);
        if (user == null) return new ErrorDataResult<UserToListDTO>(Messages.InvalidUserCredentials);

        return new SuccessDataResult<UserToListDTO>(_mapper.Map<UserToListDTO>(user));
    }
}