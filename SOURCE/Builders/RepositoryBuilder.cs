using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class RepositoryBuilder : IBuilder
{
    public void BuildSourceCode(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForRepository(model);
            SourceBuilder.Instance.AddSourceFile(Constants.RepositoryPath, $"{model.Name}Repository.cs", text);
        });
    }
}