using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Utility;
using DTO.Permission;
using DTO.Responses;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class PermissionService(IMapper mapper,
                               IPermissionRepository permissionRepository) : IPermissionService
{
    private readonly IMapper _mapper = mapper;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> AddAsync(PermissionCreateRequestDto dto)
    {
        var data = _mapper.Map<Permission>(dto);
        await _permissionRepository.AddAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _permissionRepository.GetAsync(m => m.Id == id);
        await _permissionRepository.SoftDeleteAsync(data!);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<PermissionResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _permissionRepository.GetList();
        var response = await PaginatedList<Permission>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);
        var responseDto = new PaginatedList<PermissionResponseDto>(_mapper.Map<List<PermissionResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<PermissionResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<PermissionResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<PermissionResponseDto>>(await _permissionRepository.GetListAsync());
        return new SuccessDataResult<IEnumerable<PermissionResponseDto>>(datas, EMessages.Success.Translate());
    }

    public async Task<IDataResult<PermissionByIdResponseDto>> GetAsync(Guid id)
    {
        var datas = _mapper.Map<PermissionByIdResponseDto>(await _permissionRepository.GetAsync(m => m.Id == id));
        return new SuccessDataResult<PermissionByIdResponseDto>(datas, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, PermissionUpdateRequestDto dto)
    {
        var data = _mapper.Map<Permission>(dto);
        data.Id = id;
        await _permissionRepository.UpdateAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }
}