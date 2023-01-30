using AutoMapper;
using BLL.Abstract;
using CORE.Enums;
using CORE.Helper;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DAL.Utility;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> AddAsync(UserToAddDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Username, null))
            return new ErrorResult(Messages.UserIsExist.Translate());

        dto.RoleId = dto.RoleId == 0 || !dto.RoleId.HasValue
            ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == ERole.Viewer.ToString()))?.RoleId
            : dto.RoleId;
        var data = _mapper.Map<User>(dto);

        data.Salt = SecurityHelper.GenerateSalt();
        data.Password = SecurityHelper.HashPassword(data.Password, data.Salt);

        var added = await _unitOfWork.UserRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == id);

        _unitOfWork.UserRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<List<UserToListDto>>> GetAsync()
    {
        var datas = await _unitOfWork.UserRepository.GetListAsync();

        return new SuccessDataResult<List<UserToListDto>>(_mapper.Map<List<UserToListDto>>(datas),
            Messages.Success.Translate());
    }

    public async Task<IDataResult<UserToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<UserToListDto>(await _unitOfWork.UserRepository.GetAsync(m => m.UserId == id));

        return new SuccessDataResult<UserToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(int id, UserToUpdateDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Username, id))
            return new ErrorResult(Messages.UserIsExist.Translate());

        dto.RoleId = dto.RoleId == 0 || !dto.RoleId.HasValue
            ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == ERole.Viewer.ToString()))?.RoleId
            : dto.RoleId;

        var data = _mapper.Map<User>(dto);
        data.UserId = id;

        await _unitOfWork.UserRepository.UpdateUserAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync(
        int pageIndex, int pageSize)
    {
        var datas = _unitOfWork.UserRepository.GetList();
        var response =
            await PaginatedList<User>.CreateAsync(datas.OrderBy(m => m.UserId), pageIndex,
                pageSize);

        var responseDto = new PaginatedList<UserToListDto>(
            _mapper.Map<List<UserToListDto>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);

        return new SuccessDataResult<PaginatedList<UserToListDto>>(responseDto,
            Messages.Success.Translate());
    }

    public async Task<IResult> UpdateProfilePhotoAsync(int id, string photoFileName)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == id);
        if (data == null) return new ErrorResult(Messages.InvalidUserCredentials.Translate());

        data.ImagePath = photoFileName;

        _unitOfWork.UserRepository.Update(_mapper.Map<User>(data));
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> DeleteProfilePhotoAsync(int id)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.UserId == id);
        if (data == null) return new ErrorResult(Messages.InvalidUserCredentials.Translate());

        data.ImagePath = null;

        _unitOfWork.UserRepository.Update(_mapper.Map<User>(data));
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}