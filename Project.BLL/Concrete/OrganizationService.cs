using Project.BLL.Abstract;
using Project.BLL.Mappers.GenericMapping;
using Project.Core.CustomMiddlewares.Translation;
using Project.DAL.UnitOfWorks.Abstract;
using Project.DTO.DTOs.OrganizationDto;
using Project.DTO.DTOs.Responses;
using Project.Entity.Entities;

namespace Project.BLL.Concrete;

public class OrganizationService : IOrganizationService
{
    private readonly IGenericMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public OrganizationService(IUnitOfWork unitOfWork, IGenericMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IResult> AddAsync(OrganizationToAddOrUpdateDto dto)
    {
        var entity = _mapper.Map<OrganizationToAddOrUpdateDto, Organization>(dto);
        await _unitOfWork.OrganizationRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        return new SuccessResult(Localization.Translate(Messages.Success));
    }

    public async Task<IResult> DeleteAsync(int id)
    {
        var entity = await _unitOfWork.OrganizationRepository.GetAsync(m => m.OrganizationId == id);
        entity.IsDeleted = true;
        _unitOfWork.OrganizationRepository.Update(entity);
        await _unitOfWork.CommitAsync();
        return new SuccessResult(Localization.Translate(Messages.Success));
    }

    public async Task<IDataResult<List<OrganizationToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<Organization>, List<OrganizationToListDto>>(
            await _unitOfWork.OrganizationRepository.GetListAsync());
        return new SuccessDataResult<List<OrganizationToListDto>>(datas);
    }

    public async Task<IDataResult<OrganizationToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<Organization, OrganizationToListDto>(
            await _unitOfWork.OrganizationRepository.GetAsNoTrackingAsync(m => m.OrganizationId == id));
        return new SuccessDataResult<OrganizationToListDto>(data);
    }

    public async Task<IResult> UpdateAsync(OrganizationToAddOrUpdateDto dto)
    {
        var entity = _mapper.Map<OrganizationToAddOrUpdateDto, Organization>(dto);
        _unitOfWork.OrganizationRepository.Update(entity);
        await _unitOfWork.CommitAsync();
        return new SuccessResult(Localization.Translate(Messages.Success));
    }
}