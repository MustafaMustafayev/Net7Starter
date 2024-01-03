using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IServiceBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.IServicePath, $"I{model.Name}Service.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using DTO.{entityName};
                   using DTO.Responses;
                   using DAL.EntityFramework.Utility;

                   namespace BLL.Abstract;

                   public interface I{entityName}Service
                   {
                       Task<IDataResult<PaginatedList<{entityName}ToListDto>>> GetAsPaginatedListAsync();
                       Task<IDataResult<List<{entityName}ToListDto>>> GetAsync();
                       Task<IDataResult<{entityName}ToListDto>> GetAsync(Guid id);
                       Task<IResult> AddAsync({entityName}ToAddDto dto);
                       Task<IResult> UpdateAsync(Guid id, {entityName}ToUpdateDto dto);
                       Task<IResult> SoftDeleteAsync(Guid id);
                   }

                   """;

        text = text.Replace("{entityName}", entity!.Name);
        return text;
    }
}