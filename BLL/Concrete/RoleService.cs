using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Utility;
using DTO.Permission;
using DTO.Responses;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class RoleService(IMapper mapper,
                         IRoleRepository roleRepository,
                         IPermissionRepository permissionRepository) : IRoleService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IPermissionRepository _permissionRepository = permissionRepository;

    public async Task<IResult> AddAsync(RoleCreateRequestDto dto)
    {
        var data = _mapper.Map<Role>(dto);

        var permissions = await _permissionRepository.GetListAsync(m => dto.PermissionIds!.Contains(m.Id));
        data.Permissions = permissions.ToList();

        await _roleRepository.AddAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<RoleResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
    {
        var datas = _roleRepository.GetList();

        var response = await PaginatedList<Role>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);
        var responseDto = new PaginatedList<RoleResponseDto>(_mapper.Map<List<RoleResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);

        return new SuccessDataResult<PaginatedList<RoleResponseDto>>(responseDto, EMessages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _roleRepository.GetAsync(m => m.Id == id);
        await _roleRepository.SoftDeleteAsync(data!);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<RoleResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<RoleResponseDto>>(await _roleRepository.GetListAsync());
        return new SuccessDataResult<IEnumerable<RoleResponseDto>>(datas, EMessages.Success.Translate());
    }

    public async Task<IDataResult<RoleByIdResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<RoleByIdResponseDto>(await _roleRepository.GetAsync(m => m.Id == id));
        return new SuccessDataResult<RoleByIdResponseDto>(data, EMessages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, RoleUpdateRequestDto dto)
    {
        var data = _mapper.Map<Role>(dto);
        data.Id = id;

        await _roleRepository.ClearRolePermissionsAync(id);

        var permissions = await _permissionRepository.GetListAsync(m => dto.PermissionIds!.Contains(m.Id));
        data.Permissions = permissions.ToList();
        await _roleRepository.UpdateAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<PermissionResponseDto>>> GetPermissionsAsync(Guid id)
    {
        var datas = _mapper.Map<IEnumerable<PermissionResponseDto>>((await _roleRepository.GetAsync(m => m.Id == id))?.Permissions);
        return new SuccessDataResult<IEnumerable<PermissionResponseDto>>(datas, EMessages.Success.Translate());
    }
}