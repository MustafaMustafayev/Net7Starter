using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;
using System.Text;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IUnitOfWorkBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        var newEntities = entities.ToList();
        var currentEntityNames = FileHelper.GetFileNames(Constants.EntitiesPath);

        foreach (var entityName in currentEntityNames)
        {
            if (newEntities.Any(e => e.Name == entityName)) continue;
            newEntities.Add(new Entity { Name = entityName });
        }

        SourceBuilder.Instance.AddSourceFile(Constants.IUnitOfWorkPath, "IUnitOfWork.cs",
            BuildSourceText(null, newEntities));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var properties = new StringBuilder();
        entities?.ForEach(e =>
            properties.AppendLine($"    public I{e.Name}Repository {e.Name}Repository {{ get; set; }}"));


        var text = $$"""
                     using DAL.EntityFramework.Abstract;

                     namespace DAL.EntityFramework.UnitOfWork;

                     public interface IUnitOfWork : IAsyncDisposable, IDisposable
                     {
                     {{properties}}
                         public Task CommitAsync();
                     }

                     """;

        return text;
    }
}