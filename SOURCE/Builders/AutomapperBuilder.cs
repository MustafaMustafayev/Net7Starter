using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class AutomapperBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.AutomapperPath, $"{model.Name}Mapper.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using AutoMapper;
                   using DTO.{entityName};
                   using ENTITIES.Entities{entityPath};

                   namespace BLL.Mappers;

                   public class {entityName}Mapper : Profile
                   {
                       public {entityName}Mapper()
                       {
                           CreateMap<{entityName}, {entityName}ToListDto>();
                           CreateMap<{entityName}ToAddDto, {entityName}>();
                           CreateMap<{entityName}ToUpdateDto, {entityName}>();
                       }
                   }

                   """;

        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}