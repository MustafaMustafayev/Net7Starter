using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class ServiceBuilder : ISourceBuilder, ITextBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.ServicePath, $"{model.Name}Service.cs", BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = @"
using AutoMapper;
using BLL.Abstract;
using CORE.Abstract;
using CORE.Localization;
using DAL.UnitOfWorks.Abstract;
using DTO.Responses;
using DTO.{entityName};
using ENTITIES.Entities;
using DAL.Utility;

namespace BLL.Concrete;

public class {entityName}Service : I{entityName}Service
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUtilService _utilService;
    public {entityName}Service(IMapper mapper, IUnitOfWork unitOfWork, IUtilService utilService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _utilService = utilService;
    }

    public async Task<IResult> AddAsync({entityName}ToAddDto dto)
    {
        var data = _mapper.Map<{entityName}>(dto);

        await _unitOfWork.{entityName}Repository.AddAsync(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IResult> SoftDeleteAsync(int id)
    {
        var data = await _unitOfWork.{entityName}Repository.GetAsync(m => m.{entityName}Id == id);

        _unitOfWork.{entityName}Repository.SoftDelete(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }

    public async Task<IDataResult<PaginatedList<{entityName}ToListDto>>> GetAsPaginatedListAsync()
    {
        var datas = _unitOfWork.{entityName}Repository.GetList();
        var paginationDto = _utilService.GetPagination();

        var response = await PaginatedList<{entityName}>.CreateAsync(datas.OrderBy(m => m.{entityName}Id), paginationDto.PageIndex, paginationDto.PageSize);

        var responseDto = new PaginatedList<{entityName}ToListDto>(_mapper.Map<List<{entityName}ToListDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);

        return new SuccessDataResult<PaginatedList<{entityName}ToListDto>>(responseDto, Messages.Success.Translate());
    }

    public async Task<IDataResult<List<{entityName}ToListDto>>> GetAsync()
    {
        var datas = _mapper.Map<List<{entityName}ToListDto>>(await _unitOfWork.{entityName}Repository.GetListAsync());

        return new SuccessDataResult<List<{entityName}ToListDto>>(datas, Messages.Success.Translate());
    }

    public async Task<IDataResult<{entityName}ToListDto>> GetAsync(int id)
    {
        var data = _mapper.Map<{entityName}ToListDto>(await _unitOfWork.{entityName}Repository.GetAsync(m => m.{entityName}Id == id));

        return new SuccessDataResult<{entityName}ToListDto>(data, Messages.Success.Translate());
    }

    public async Task<IResult> UpdateAsync(int id, {entityName}ToUpdateDto dto)
    {
        var data = _mapper.Map<{entityName}>(dto);
        data.{entityName}Id = id;

        _unitOfWork.{entityName}Repository.Update(data);
        await _unitOfWork.CommitAsync();

        return new SuccessResult(Messages.Success.Translate());
    }
}
";

        text = text.Replace("{entityName}", entity!.Name);
        return text;
    }
}