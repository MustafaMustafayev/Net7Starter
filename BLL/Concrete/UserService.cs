using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Helper;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DAL.Utility;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using ENTITIES.Enums;

namespace BLL.Concrete;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IUtilService utilService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(UserToAddDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Username, null))
            return new ErrorResult(Messages.UserIsExist.Translate());

        dto.RoleId = dto.RoleId == 0 || !dto.RoleId.HasValue
            ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == UserType.Viewer.ToString()))?.Id
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
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.Id == id);

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
        var data = _mapper.Map<UserToListDto>(await _unitOfWork.UserRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<UserToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(int id, UserToUpdateDto dto)
    {
        if (await _unitOfWork.UserRepository.IsUserExistAsync(dto.Username, id))
            return new ErrorResult(Messages.UserIsExist.Translate());

        dto.RoleId = dto.RoleId == 0 || !dto.RoleId.HasValue
            ? (await _unitOfWork.RoleRepository.GetAsync(m => m.Key == UserType.Viewer.ToString()))?.Id
            : dto.RoleId;

        var data = _mapper.Map<User>(dto);
        data.Id = id;

        await _unitOfWork.UserRepository.UpdateUserAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<UserToListDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.UserRepository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<User>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<UserToListDto>(
            _mapper.Map<List<UserToListDto>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);

        return new SuccessDataResult<PaginatedList<UserToListDto>>(responseDto,
            Messages.Success.Translate());
    }

    public async Task<IResult> UpdateImageAsync(int id, int? imageId)
    {
        var data = await _unitOfWork.UserRepository.GetAsync(m => m.Id == id);
        if (data == null) return new ErrorResult(Messages.InvalidUserCredentials.Translate());

        data.ImageId = imageId;

        _unitOfWork.UserRepository.Update(_mapper.Map<User>(data));
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}