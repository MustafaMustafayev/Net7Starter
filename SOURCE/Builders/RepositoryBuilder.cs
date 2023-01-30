using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class RepositoryBuilder : ISourceBuilder, ITextBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities.ForEach(model =>
            SourceBuilder.Instance.AddSourceFile(Constants.RepositoryPath, $"{model.Name}Repository.cs", BuildSourceText(model, null)));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = @"
using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.GenericRepositories.Concrete;
using ENTITIES.Entities;

namespace DAL.Concrete;

public class {entityName}Repository : GenericRepository<{entityName}>, I{entityName}Repository
{
    private readonly DataContext _dataContext;

    public {entityName}Repository(DataContext dataContext)
        : base(dataContext)
    {
        _dataContext = dataContext;
    }
}
";
        text = text.Replace("{entityName}", entity!.Name);

        return text;
    }
}