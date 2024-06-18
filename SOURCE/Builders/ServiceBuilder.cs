using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
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
                //&& w.Options.BuildUnitOfWork
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
                   using CORE.Localization;
                   using DAL.EntityFramework.Abstract;
                   using DAL.EntityFramework.Utility;
                   using DTO.Responses;
                   using DTO.{entityName};
                   using ENTITIES.Entities{entityPath};

                   namespace BLL.Concrete;

                   public class {entityName}Service(IMapper mapper,
                                                    I{entityName}Repository {entityNameLower}Repository) : I{entityName}Service
                   {
                       private readonly IMapper _mapper = mapper;
                       private readonly I{entityName}Repository _{entityNameLower}Repository = {entityNameLower}Repository;
                   
                       public async Task<IResult> AddAsync({entityName}CreateRequestDto dto)
                       {
                           var data = _mapper.Map<{entityName}>(dto);       
                           await _{entityNameLower}Repository.AddAsync(data);
                   
                           return new SuccessResult(EMessages.Success.Translate());
                       }
                   
                       public async Task<IResult> SoftDeleteAsync(Guid id)
                       {
                           var data = await _{entityNameLower}Repository.GetAsync(m => m.Id == id);
                           if (data is null)
                           {
                               return new ErrorResult(EMessages.DataNotFound.Translate());
                           }
                           await _{entityNameLower}Repository.SoftDeleteAsync(data);
                   
                           return new SuccessResult(EMessages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<PaginatedList<{entityName}ResponseDto>>> GetAsPaginatedListAsync(int pageIndex, int pageSize)
                       {
                           var datas = _{entityNameLower}Repository.GetList();
               
                           var response = await PaginatedList<{entityName}>.CreateAsync(datas.OrderBy(m => m.Id), pageIndex, pageSize);                  
                           var responseDto = new PaginatedList<{entityName}ResponseDto>(_mapper.Map<List<{entityName}ResponseDto>>(response.Datas), response.TotalRecordCount, response.PageIndex, pageSize);
                   
                           return new SuccessDataResult<PaginatedList<{entityName}ResponseDto>>(responseDto, EMessages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<IEnumerable<{entityName}ResponseDto>>> GetAsync()
                       {
                           var datas = _mapper.Map<IEnumerable<{entityName}ResponseDto>>(await _{entityNameLower}Repository.GetListAsync());                  
                           return new SuccessDataResult<IEnumerable<{entityName}ResponseDto>>(datas, EMessages.Success.Translate());
                       }
                   
                       public async Task<IDataResult<{entityName}ByIdResponseDto>> GetAsync(Guid id)
                       {
                           var data = _mapper.Map<{entityName}ByIdResponseDto>(await _{entityNameLower}Repository.GetAsync(m => m.Id == id));              
                           return new SuccessDataResult<{entityName}ByIdResponseDto>(data, EMessages.Success.Translate());
                       }
                   
                       public async Task<IResult> UpdateAsync(Guid id, {entityName}UpdateRequestDto dto)
                       {
                           var data = _mapper.Map<{entityName}>(dto);
                           data.Id = id;              
                           _{entityNameLower}Repository.UpdateAsync(data);
                   
                           return new SuccessResult(EMessages.Success.Translate());
                       }
                   }
                   """;

        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityNameLower}", entity.Name.FirstCharToLowerCase());
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}