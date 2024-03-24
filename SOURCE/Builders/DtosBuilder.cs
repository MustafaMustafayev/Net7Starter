using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;
using System.Reflection;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class DtosBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        //entities.ForEach(model => SourceBuilder.Instance
        //    .AddSourceFile(Constants.DtoPath.Replace("{entityName}", model.Name),
        //        $"{model.Name}Dtos.cs", BuildSourceText(model, null)));

        foreach (var entity in entities)
        {
            SourceBuilder.Instance.AddSourceFile(
                Constants.DtoPath.Replace("{entityName}", entity.Name),
                $"{entity.Name}ToAddDto.cs", GenerateContent(entity.Name, $"{entity.Name}ToAddDto"));

            SourceBuilder.Instance.AddSourceFile(
                Constants.DtoPath.Replace("{entityName}", entity.Name),
                $"{entity.Name}ToUpdateDto.cs", GenerateContent(entity.Name, $"{entity.Name}ToUpdateDto"));

            SourceBuilder.Instance.AddSourceFile(
                Constants.DtoPath.Replace("{entityName}", entity.Name),
                $"{entity.Name}ToListDto.cs", GenerateContent(entity.Name, $"{entity.Name}ToListDto"));
        }
    }

    private string GetProperties(string entityName)
    {

        return string.Empty;
    }

    private string GenerateContent(string name, string className)
    {
        var text = """
                   namespace DTO.{0};

                   public record {1}
                   {2}
                   
                   {3}

                   """;

        return string.Format(text,name,className,"{","}");
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   namespace DTO.{entityName};

                   public record {entityName}ToAddDto();
                   public record {entityName}ToUpdateDto();
                   public record {entityName}ToListDto(Guid Id);

                   """;

        text = text.Replace("{entityName}", entity!.Name);
        return text;
    }

}