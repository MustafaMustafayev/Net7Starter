using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IRepositoryBuilder : ISourceBuilder, ITextBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.IRepositoryPath, $"I{model.Name}Repository.cs",
                BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = @"
using DAL.GenericRepositories.Abstract;
using ENTITIES.Entities;

namespace DAL.EntityFramework.Abstract;

public interface I{entityName}Repository : IGenericRepository<{entityName}>
{
}
";
        text = text.Replace("{entityName}", entity!.Name);

        return text;
    }
}