using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class AutomapperBuilder : ISourceBuilder, ITextBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.AutomapperPath, $"{model.Name}Mapper.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = @"
using AutoMapper;
using DTO.{entityName};
using ENTITIES.Entities;

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
";

        text = text.Replace("{entityName}", entity!.Name);
        return text;
    }
}