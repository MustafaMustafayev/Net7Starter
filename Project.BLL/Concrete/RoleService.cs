using Project.BLL.Abstract;
using Project.BLL.Mappers.GenericMapping;
using Project.Core.CustomMiddlewares.Translation;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.Responses;
using Project.DTO.DTOs.RoleDto;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

public class RoleService : IRoleService
{
    private readonly IGenericMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork, IGenericMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IDataResult<Result>> AddAsync(RoleToAddOrUpdateDto dto)
    {
        var entity = _mapper.Map<RoleToAddOrUpdateDto, Role>(dto);
        await _unitOfWork.RoleRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        return new SuccessDataResult<Result>(null, Localization.Translate(Messages.Success));
    }

    public async Task<IResult> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.RoleRepository.GetAsync(m => m.RoleId == id);
        entity.IsDeleted = true;
        _unitOfWork.RoleRepository.Update(entity);
        await _unitOfWork.CommitAsync();
        return new SuccessResult(Localization.Translate(Messages.Success));
    }

    public async Task<IDataResult<List<RoleToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<Role>, List<RoleToListDto>>(await _unitOfWork.RoleRepository.GetListAsync());
        return new SuccessDataResult<List<RoleToListDto>>(datas);
    }

    public async Task<IDataResult<RoleToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<Role, RoleToListDto>(
            await _unitOfWork.RoleRepository.GetAsNoTrackingAsync(m => m.RoleId == id));
        return new SuccessDataResult<RoleToListDto>(data);
    }

    public async Task<IDataResult<Result>> UpdateAsync(RoleToAddOrUpdateDto dto)
    {
        var entity = _mapper.Map<RoleToAddOrUpdateDto, Role>(dto);
        _unitOfWork.RoleRepository.Update(entity);
        await _unitOfWork.CommitAsync();
        return new SuccessDataResult<Result>(null, Localization.Translate(Messages.Success));
    }
}