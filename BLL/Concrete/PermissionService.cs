using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DAL.EntityFramework.Utility;
using DTO.Permission;
using DTO.Responses;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class PermissionService : IPermissionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;

    public PermissionService(IUnitOfWork unitOfWork, IMapper mapper, IUtilService utilService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync(PermissionCreateRequestDto dto)
    {
        var data = _mapper.Map<Permission>(dto);

        await _unitOfWork.PermissionRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.PermissionRepository.GetAsync(m => m.Id == id);
        _unitOfWork.PermissionRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<PermissionResponseDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.PermissionRepository.GetList();
        var paginationDto = _utilService.GetPagination();
        var response = await PaginatedList<Permission>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex,
            paginationDto.PageSize);

        var responseDto = new PaginatedList<PermissionResponseDto>(_mapper.Map<List<PermissionResponseDto>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);

        return new SuccessDataResult<PaginatedList<PermissionResponseDto>>(responseDto, Messages.Success.Translate());
    }

    public async Task<IDataResult<List<PermissionResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<PermissionResponseDto>>(await _unitOfWork.PermissionRepository.GetListAsync());

        return new SuccessDataResult<List<PermissionResponseDto>>(datas, Messages.Success.Translate());

    }

    public async Task<IDataResult<PermissionByIdResponseDto>> GetAsync(Guid id)
    {
        var datas = _mapper.Map<PermissionByIdResponseDto>(await _unitOfWork.PermissionRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<PermissionByIdResponseDto>(datas, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid permissionId, PermissionUpdateRequestDto dto)
    {
        var data = _mapper.Map<Permission>(dto);
        data.Id = permissionId;

        _unitOfWork.PermissionRepository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}