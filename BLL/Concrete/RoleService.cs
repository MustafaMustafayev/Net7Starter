using AutoMapper;
using BLL.Abstract;
using CORE.Localization;
using DAL.EntityFramework.UnitOfWork;
using DTO.Permission;
using DTO.Responses;
using DTO.Role;
using ENTITIES.Entities;

namespace BLL.Concrete;

public class RoleService : IRoleService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> AddAsync(RoleCreateRequestDto dto)
    {
        var data = _mapper.Map<Role>(dto);

        var permissions = await _unitOfWork.PermissionRepository.GetListAsync(m => dto.PermissionIds!.Contains(m.Id));
        data.Permissions = permissions.ToList();

        await _unitOfWork.RoleRepository.AddRoleAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(Guid id)
    {
        var data = await _unitOfWork.RoleRepository.GetAsync(m => m.Id == id);

        _unitOfWork.RoleRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<RoleResponseDto>>> GetAsync()
    {
        var datas = _mapper.Map<IEnumerable<RoleResponseDto>>(await _unitOfWork.RoleRepository.GetListAsync());
        return new SuccessDataResult<IEnumerable<RoleResponseDto>>(datas, Messages.Success.Translate());
    }

    public Task<IDataResult<IQueryable<Role>>> GraphQlGetAsync()
    {
        return Task.FromResult<IDataResult<IQueryable<Role>>>(new SuccessDataResult<IQueryable<Role>>( _unitOfWork.RoleRepository.GetList()!,Messages.Success.Translate()));
    }

    public async Task<IDataResult<RoleByIdResponseDto>> GetAsync(Guid id)
    {
        var data = _mapper.Map<RoleByIdResponseDto>(await _unitOfWork.RoleRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<RoleByIdResponseDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(Guid id, RoleUpdateRequestDto dto)
    {
        var data = _mapper.Map<Role>(dto);
        data.Id = id;

        await _unitOfWork.RoleRepository.ClearRolePermissionsAync(id);

        var permissions = await _unitOfWork.PermissionRepository.GetListAsync(m => dto.PermissionIds!.Contains(m.Id));
        data.Permissions = permissions.ToList();
        _unitOfWork.RoleRepository.UpdateRole(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<IEnumerable<PermissionResponseDto>>> GetPermissionsAsync(Guid id)
    {
        var datas = _mapper.Map<IEnumerable<PermissionResponseDto>>((await _unitOfWork.RoleRepository.GetAsync(m => m.Id == id))!.Permissions);

        return new SuccessDataResult<IEnumerable<PermissionResponseDto>>(datas, Messages.Success.Translate());
    }
}