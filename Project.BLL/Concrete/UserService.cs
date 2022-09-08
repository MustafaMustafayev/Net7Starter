using AutoMapper;
using Project.BLL.Abstract;
using Project.Core.Constants;
using Project.Core.Helper;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.AuthDTOs;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.UserDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<Result>> AddAsync(UserToAddDTO dto)
    {
        User entity = _mapper.Map<User>(dto);
        entity.Salt = SecurityHelper.GenerateSalt();
        entity.Password = SecurityHelper.HashPassword(entity.Password, entity.Salt);
        await _unitOfWork.UserRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        return new SuccessDataResult<Result>(null, Messages.Success);
    }

    public async Task DeleteAsync(int id)
    {
        User entity = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == id);
        entity.IsDeleted = true;
        _unitOfWork.UserRepository.Update(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IDataResult<List<UserToListDTO>>> GetAsync()
    {
        List<UserToListDTO> datas = _mapper.Map<List<UserToListDTO>>(await _unitOfWork.UserRepository.GetListAsync());
        return new SuccessDataResult<List<UserToListDTO>>(datas);
    }

    public async Task<IDataResult<UserToListDTO>> GetAsync(int id)
    {
        UserToListDTO data = _mapper.Map<UserToListDTO>(await _unitOfWork.UserRepository.GetAsNoTrackingAsync(m => m.UserId == id));
        return new SuccessDataResult<UserToListDTO>(data);
    }

    public async Task ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
    {
        User entity = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == resetPasswordDto.UserId);
        entity.Salt = SecurityHelper.GenerateSalt();
        entity.Password = SecurityHelper.HashPassword(resetPasswordDto.Password, entity.Salt);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IDataResult<Result>> UpdateAsync(int id, UserToUpdateDTO dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.PIN, id))
            return new ErrorDataResult<Result>(Messages.UserIsExist);

        User data = _mapper.Map<User>(dto);

        await _unitOfWork.UserRepository.UpdateUserAsync(data);

        await _unitOfWork.CommitAsync();
        return new SuccessDataResult<Result>(Messages.Success);
    }
}