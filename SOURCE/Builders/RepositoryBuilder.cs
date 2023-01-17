using SourceBuilder.Builders.Abstract;
using SourceBuilder.Helpers;
using SourceBuilder.Models;

namespace SourceBuilder.Builders;

// ReSharper disable once UnusedType.Global
public class RepositoryBuilder : IBuilder
{
    public void Build(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForRepository(model);
            Workers.SourceBuilder.Instance.AddSourceFile(Constants.RepositoryPath, $"{model.Name}Repository.cs", text);
        });
    }
}