using SourceBuilder.Builders.Abstract;
using SourceBuilder.Helpers;
using SourceBuilder.Models;

namespace SourceBuilder.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IServiceBuilder : IBuilder
{
    public void Build(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForIEntityService(model);
            Workers.SourceBuilder.Instance.AddSourceFile(Constants.IServicePath, $"I{model.Name}Service.cs", text);
        });
    }
}