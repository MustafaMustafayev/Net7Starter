using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IRepositoryBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities
            .Where(w => w.Options.BuildRepository)
            .ToList()
            .ForEach(model =>
                SourceBuilder.Instance.AddSourceFile(Constants.I_REPOSITORY_PATH, $"I{model.Name}Repository.cs",
                    BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using DAL.EntityFramework.GenericRepository;
                   using ENTITIES.Entities{entityPath};

                   namespace DAL.EntityFramework.Abstract;

                   public interface I{entityName}Repository : IGenericRepository<{entityName}>
                   {
                   }
                   """;
        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}