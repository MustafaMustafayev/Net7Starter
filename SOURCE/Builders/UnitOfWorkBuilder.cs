using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;
using System.Text;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class UnitOfWorkBuilder : ISourceBuilder
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

        SourceBuilder.Instance.AddSourceFile(Constants.UnitOfWorkPath, "UnitOfWork.cs",
            BuildSourceText(null, newEntities));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var constructorArguments = new StringBuilder();
        entities!.ForEach(e =>
        {
            if (entities.Last() == e)
                constructorArguments.Append(
                    $"        I{e.Name}Repository {e.Name.FirstCharToLowerCase()}Repository");
            else
                constructorArguments.AppendLine(
                    $"        I{e.Name}Repository {e.Name.FirstCharToLowerCase()}Repository,");
        });

        var constructorSetters = new StringBuilder();
        entities.ForEach(e =>
        {
            if (entities.Last() == e)
                constructorSetters.Append(
                    $"        {e.Name}Repository = {e.Name.FirstCharToLowerCase()}Repository;");
            else
                constructorSetters.AppendLine(
                    $"        {e.Name}Repository = {e.Name.FirstCharToLowerCase()}Repository;");
        });

        var properties = new StringBuilder();
        entities.ForEach(e =>
            properties.AppendLine($"    public I{e.Name}Repository {e.Name}Repository {{ get; set; }}"));


        var text = $$"""
                     using DAL.EntityFramework.Abstract;
                     using DAL.EntityFramework.Context;

                     namespace DAL.EntityFramework.UnitOfWork;

                     public class UnitOfWork : IUnitOfWork
                     {
                         private readonly DataContext _dataContext;
                     
                         private bool _isDisposed;
                     
                         public UnitOfWork(
                             DataContext dataContext,
                     {{constructorArguments}}
                         )
                         {
                             _dataContext = dataContext;
                     {{constructorSetters}}
                         }

                     {{properties}}
                         public async Task CommitAsync()
                         {
                             await _dataContext.SaveChangesAsync();
                         }
                     
                         public async ValueTask DisposeAsync()
                         {
                             if (!_isDisposed)
                             {
                                 _isDisposed = true;
                                 await DisposeAsync(true);
                                 GC.SuppressFinalize(this);
                             }
                         }
                     
                         public void Dispose()
                         {
                             if (_isDisposed) return;
                             _isDisposed = true;
                             Dispose(true);
                             GC.SuppressFinalize(this);
                         }
                     
                         protected virtual void Dispose(bool disposing)
                         {
                             if (disposing) _dataContext.Dispose();
                         }
                     
                         private async ValueTask DisposeAsync(bool disposing)
                         {
                             if (disposing) await _dataContext.DisposeAsync();
                         }
                     }

                     """;

        return text;
    }
}