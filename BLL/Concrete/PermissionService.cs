using AutoMapper;
using BLL.Abstract;
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

    public PermissionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        _unitOfWork.PermissionRepository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<PermissionToListDto>>> GetAsPaginatedListAsync(int pageIndex,
        int pageSize)
    {
        var datas = _unitOfWork.PermissionRepository.GetAsNoTrackingList();
        var response =
            await PaginatedList<Permission>.CreateAsync(datas.OrderBy(m => m.PermissionId),
                pageIndex, pageSize);

        var responseDto = new PaginatedList<PermissionToListDto>(
            _mapper.Map<List<PermissionToListDto>>(response.Datas),
            response.TotalRecordCount, response.PageIndex, response.TotalPageCount);

        return new SuccessDataResult<PaginatedList<PermissionToListDto>>(responseDto,
            Messages.Success.Translate());
    }

    public async Task<IDataResult<List<PermissionToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<PermissionToListDto>>(_unitOfWork.PermissionRepository.GetAsNoTrackingList()
            .ToList());

        return new SuccessDataResult<List<PermissionToListDto>>(datas,
            Messages.Success.Translate());
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