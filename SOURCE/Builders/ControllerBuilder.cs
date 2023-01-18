using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class ControllerBuilder : IBuilder
{
    public void BuildSourceCode(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForEntityController(model);
            SourceBuilder.Instance.AddSourceFile(Constants.ControllerPath, $"{model.Name}Controller.cs", text);
        });
    }
}