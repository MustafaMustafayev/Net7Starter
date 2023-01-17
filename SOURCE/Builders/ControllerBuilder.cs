using SourceBuilder.Builders.Abstract;
using SourceBuilder.Helpers;
using SourceBuilder.Models;

namespace SourceBuilder.Builders;

// ReSharper disable once UnusedType.Global
public class ControllerBuilder : IBuilder
{
    public void Build(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForEntityController(model);
            Workers.SourceBuilder.Instance.AddSourceFile(Constants.ControllerPath, $"{model.Name}Controller.cs", text);
        });
    }
}