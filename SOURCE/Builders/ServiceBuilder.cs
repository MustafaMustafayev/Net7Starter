using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class ServiceBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities
            .Where(w =>
                w.Options.BuildService
                && w.Options.BuildUnitOfWork
                && w.Options.BuildRepository)
            .ToList().ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.ServicePath, $"{model.Name}Service.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using AutoMapper;
                   using BLL.Abstract;
                   using CORE.Abstract;
                   using CORE.Localization;
                   using DTO.Responses;
                   using DTO.{entityName};
                   using ENTITIES.Entities{entityPath};
                   using DAL.EntityFramework.Utility;
                   using DAL.EntityFramework.UnitOfWork;

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
                   
                       public async Task<IResult> AddAsync({entityName}CreateRequestDto dto)
                       {
                           var data = _mapper.Map<{entityName}>(dto);
                   
                           await _unitOfWork.{entityName}Repository.AddAsync(data);
                           await _unitOfWork.CommitAsync();
                   
                           return new SuccessResult(Messages.Success.Translate());
                       }
                   
                       public async Task<IResult> SoftDeleteAsync(Guid id)
                       {
                           var data = await _unitOfWork.{entityName}Repository.GetAsync(m => m.Id == id);
                   
                           _unitOfWork.{entityName}Repository.SoftDelete(data);
                           await _unitOfWork.CommitAsync();
                   
                           return new SuccessResult(Messages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<PaginatedList<{entityName}ResponseDto>>> GetAsPaginatedListAsync()
                       {
                           var datas = _unitOfWork.{entityName}Repository.GetList();
                           var paginationDto = _utilService.GetPagination();
                   
                           var response = await PaginatedList<{entityName}>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex, paginationDto.PageSize);
                   
                           var responseDto = new PaginatedList<{entityName}ResponseDto>(_mapper.Map<List<{entityName}ResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);
                   
                           return new SuccessDataResult<PaginatedList<{entityName}ResponseDto>>(responseDto, Messages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<List<{entityName}ResponseDto>>> GetAsync()
                       {
                           var datas = _mapper.Map<List<{entityName}ResponseDto>>(await _unitOfWork.{entityName}Repository.GetListAsync());
                   
                           return new SuccessDataResult<List<{entityName}ResponseDto>>(datas, Messages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<{entityName}ByIdResponseDto>> GetAsync(Guid id)
                       {
                           var data = _mapper.Map<{entityName}ByIdResponseDto>(await _unitOfWork.{entityName}Repository.GetAsync(m => m.Id == id));
                   
                           return new SuccessDataResult<{entityName}ByIdResponseDto>(data, Messages.Success.Translate());
                       }
                   
                       public async Task<IResult> UpdateAsync(Guid id, {entityName}UpdateRequestDto dto)
                       {
                           var data = _mapper.Map<{entityName}>(dto);
                           data.Id = id;
                   
                           _unitOfWork.{entityName}Repository.Update(data);
                           await _unitOfWork.CommitAsync();
                   
                           return new SuccessResult(Messages.Success.Translate());
                       }
                   }

                   """;

        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}