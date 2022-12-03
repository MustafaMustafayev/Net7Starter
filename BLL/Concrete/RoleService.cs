using AutoMapper;
using BLL.Abstract;
using CORE.Middlewares.Translation;
using DAL.UnitOfWorks.Abstract;
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
        var entity = _mapper.Map<Role>(dto);

        await _unitOfWork.RoleRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Localization.Translate(Messages.Success));
    }

    public async Task<IResult> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.RoleRepository.GetAsync(m => m.RoleId == id);
        entity!.IsDeleted = true;

        _unitOfWork.RoleRepository.Update(entity);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Localization.Translate(Messages.Success));
    }

    public async Task<IDataResult<List<RoleToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<RoleToListDto>>(_unitOfWork.RoleRepository.GetList());

        return new SuccessDataResult<List<RoleToListDto>>(datas);
    }

    public async Task<IDataResult<IQueryable<Role>>> GraphQlGetAsync()
    {
        return new SuccessDataResult<IQueryable<Role>>(_unitOfWork.RoleRepository.GetList());
    }

    public async Task<IDataResult<RoleToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<RoleToListDto>(
            (await _unitOfWork.RoleRepository.GetAsNoTrackingAsync(m => m.RoleId == id))!);

        return new SuccessDataResult<RoleToListDto>(data);
    }

    public async Task<IResult> UpdateAsync(RoleToUpdateDto dto)
    {
        var entity = _mapper.Map<Role>(dto);

        _unitOfWork.RoleRepository.Update(entity);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Localization.Translate(Messages.Success));
    }
}