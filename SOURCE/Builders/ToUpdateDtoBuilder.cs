﻿using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class ToUpdateDtoBuilder : ISourceBuilder, ITextBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model => SourceBuilder.Instance
            .AddSourceFile(Constants.DtoPath.Replace("{entityName}", model.Name),
                $"{model.Name}ToUpdateDto.cs", BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = @"
namespace DTO.{entityName};

public record {entityName}ToUpdateDto
{
    public int {entityName}Id { get; set; }
    ...
}
";

        text = text.Replace("{entityName}", entity!.Name);
        return text;
    }
}