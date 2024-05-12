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
                && w.Options.BuildDto
                && w.Options.BuildUnitOfWork
                && w.Options.BuildRepository)
            .ToList().ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.SERVICE_PATH, $"{model.Name}Service.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using AutoMapper;
                   using BLL.Abstract;
                   using CORE.Abstract;
                   using CORE.Localization;
                   using DAL.EntityFramework.UnitOfWork;
                   using DAL.EntityFramework.Utility;
                   using DTO.Responses;
                   using DTO.{entityName};
                   using ENTITIES.Entities{entityPath};

                   namespace BLL.Concrete;

                   public class {entityName}Service(IMapper mapper, IUnitOfWork unitOfWork, IUtilService utilService) : I{entityName}Service
                   {
                       private readonly IMapper _mapper = mapper;
                       private readonly IUnitOfWork _unitOfWork = unitOfWork;
                       private readonly IUtilService _utilService = utilService;
                   
                       public async Task<IResult> AddAsync({entityName}CreateRequestDto dto)
                       {
                           var data = _mapper.Map<{entityName}>(dto);
                   
                           await _unitOfWork.{entityName}Repository.AddAsync(data);
                           await _unitOfWork.CommitAsync();
                   
                           return new SuccessResult(EMessages.Success.Translate());
                       }
                   
                       public async Task<IResult> SoftDeleteAsync(Guid id)
                       {
                           var data = await _unitOfWork.{entityName}Repository.GetAsync(m => m.Id == id);
                           if (data is null)
                           {
                               return new ErrorResult(EMessages.DataNotFound.Translate());
                           }
                           _unitOfWork.{entityName}Repository.SoftDelete(data);
                           await _unitOfWork.CommitAsync();
                   
                           return new SuccessResult(EMessages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<PaginatedList<{entityName}ResponseDto>>> GetAsPaginatedListAsync()
                       {
                           var datas = _unitOfWork.{entityName}Repository.GetList();
                           var paginationDto = _utilService.GetPagination();
                   
                           var response = await PaginatedList<{entityName}>.CreateAsync(datas.OrderBy(m => m.Id), paginationDto.PageIndex, paginationDto.PageSize);
                   
                           var responseDto = new PaginatedList<{entityName}ResponseDto>(_mapper.Map<List<{entityName}ResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, paginationDto.PageSize);
                   
                           return new SuccessDataResult<PaginatedList<{entityName}ResponseDto>>(responseDto, EMessages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<IEnumerable<{entityName}ResponseDto>>> GetAsync()
                       {
                           var datas = _mapper.Map<IEnumerable<{entityName}ResponseDto>>(await _unitOfWork.{entityName}Repository.GetListAsync());
                   
                           return new SuccessDataResult<IEnumerable<{entityName}ResponseDto>>(datas, EMessages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<{entityName}ByIdResponseDto>> GetAsync(Guid id)
                       {
                           var data = _mapper.Map<{entityName}ByIdResponseDto>(await _unitOfWork.{entityName}Repository.GetAsync(m => m.Id == id));
                   
                           return new SuccessDataResult<{entityName}ByIdResponseDto>(data, EMessages.Success.Translate());
                       }
                   
                       public async Task<IResult> UpdateAsync(Guid id, {entityName}UpdateRequestDto dto)
                       {
                           var data = _mapper.Map<{entityName}>(dto);
                           data.Id = id;
                   
                           _unitOfWork.{entityName}Repository.Update(data);
                           await _unitOfWork.CommitAsync();
                   
                           return new SuccessResult(EMessages.Success.Translate());
                       }
                   }
                   """;

        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}