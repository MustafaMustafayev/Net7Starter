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

    public async Task<IResult> AddAsync(RoleToAddDto dto)
    {
        var data = _mapper.Map<Role>(dto);

        var permissions =
            await _unitOfWork.PermissionRepository.GetListAsync(m => dto.PermissionIds!.Contains(m.Id));
        data.Permissions = permissions;

        await _unitOfWork.RoleRepository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.RoleRepository.GetAsync(m => m.Id == id);

        _unitOfWork.RoleRepository.SoftDelete(data!);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<List<RoleToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<RoleToListDto>>(await _unitOfWork.RoleRepository.GetListAsync());

        return new SuccessDataResult<List<RoleToListDto>>(datas, Messages.Success.Translate());
    }

    public Task<IDataResult<IQueryable<Role>>> GraphQlGetAsync()
    {
        return Task.FromResult<IDataResult<IQueryable<Role>>>(new SuccessDataResult<IQueryable<Role>>(_unitOfWork.RoleRepository.GetList()!,
            Messages.Success.Translate()));
    }

    public async Task<IDataResult<RoleToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<RoleToListDto>(await _unitOfWork.RoleRepository.GetAsync(m => m.Id == id));

        return new SuccessDataResult<RoleToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(int id, RoleToUpdateDto dto)
    {
        var data = _mapper.Map<Role>(dto);
        data.Id = id;

        await _unitOfWork.RoleRepository.ClearRolePermissionsAync(id);

        var permissions = await _unitOfWork.PermissionRepository.GetListAsync(m => dto.PermissionIds!.Contains(m.Id));
        data.Permissions = permissions;
        _unitOfWork.RoleRepository.UpdateRole(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<List<PermissionToListDto>>> GetPermissionsAsync(int id)
    {
        var datas = _mapper.Map<List<PermissionToListDto>>(
            (await _unitOfWork.RoleRepository.GetAsync(m => m.Id == id))!.Permissions);

        return new SuccessDataResult<List<PermissionToListDto>>(datas,
            Messages.Success.Translate());
    }
}