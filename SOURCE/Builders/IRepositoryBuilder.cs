using SourceBuilder.Builders.Abstract;
using SourceBuilder.Helpers;
using SourceBuilder.Models;

namespace SourceBuilder.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IRepositoryBuilder : IBuilder
{
    public void Build(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForIRepository(model);
            Workers.SourceBuilder.Instance.AddSourceFile(Constants.IRepositoryPath, $"I{model.Name}Repository.cs", text);
        });
    }
}