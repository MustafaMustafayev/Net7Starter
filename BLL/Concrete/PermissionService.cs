using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DAL.EntityFramework.Utility;
using DTO.Permission;
using DTO.Responses;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class PermissionService(IMapper mapper,
                               IUnitOfWork unitOfWork) : IPermissionService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IResult> AddAsync(PermissionCreateRequestDto dto)
    {
        var data = _mapper.Map<Permission>(dto);

        await _unitOfWork.PermissionRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.PermissionRepository.GetAsync(m => m.Id == id);
        _unitOfWork.PermissionRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<PermissionResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _unitOfWork.PermissionRepository.GetList();
        var response = await PaginatedList<Permission>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);

        var responseDto = new PaginatedList<PermissionResponseDto>(_mapper.Map<List<PermissionResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<PermissionResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<PermissionResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<PermissionResponseDto>>(await _unitOfWork.PermissionRepository.GetListAsync());

        return new SuccessDataResult<IEnumerable<PermissionResponseDto>>(datas, EMessages.Success.Translate());

    }

    public async Task<IDataResult<PermissionByIdResponseDto>> GetAsync(Guid id)
    {
        var datas = _mapper.Map<PermissionByIdResponseDto>(await _unitOfWork.PermissionRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<PermissionByIdResponseDto>(datas, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid permissionId, PermissionUpdateRequestDto dto)
    {
        var data = _mapper.Map<Permission>(dto);
        data.Id = permissionId;

        _unitOfWork.PermissionRepository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(EMessages.Success.Translate());
    }
}