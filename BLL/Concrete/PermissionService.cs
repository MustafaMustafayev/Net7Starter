using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DAL.Utility;
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

    public async Task<IResult> AddAsync(PermissionToAddDto dto)
    {
        var data = _mapper.Map<Permission>(dto);

        await _unitOfWork.PermissionRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.PermissionRepository.GetAsync(m => m.PermissionId == id);
        _unitOfWork.PermissionRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<PermissionToListDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.PermissionRepository.GetAsNoTrackingList();
        var paginationDto = _utilService.GetPagination();
        var response = await PaginatedList<Permission>.CreateAsync(datas.OrderBy(m => m.PermissionId), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<PermissionToListDto>(_mapper.Map<List<PermissionToListDto>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);

        return new SuccessDataResult<PaginatedList<PermissionToListDto>>(responseDto, Messages.Success.Translate());
    }

    public Task<IDataResult<List<PermissionToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<PermissionToListDto>>(_unitOfWork.PermissionRepository.GetAsNoTrackingList()
            .ToList());

        return Task.FromResult<IDataResult<List<PermissionToListDto>>>(new SuccessDataResult<List<PermissionToListDto>>(datas,
            Messages.Success.Translate()));
    }

    public async Task<IDataResult<PermissionToListDto>> GetAsync(int id)
    {
        var datas = _mapper.Map<PermissionToListDto>(
            await _unitOfWork.PermissionRepository.GetAsNoTrackingAsync(m => m.PermissionId == id));

        return new SuccessDataResult<PermissionToListDto>(datas, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(int permissionId, PermissionToUpdateDto dto)
    {
        var data = _mapper.Map<Permission>(dto);
        data.PermissionId = permissionId;

        _unitOfWork.PermissionRepository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}