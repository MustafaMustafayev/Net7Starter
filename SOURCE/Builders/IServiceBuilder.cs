using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IServiceBuilder : IBuilder
{
    public void BuildSourceCode(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForIEntityService(model);
            SourceBuilder.Instance.AddSourceFile(Constants.IServicePath, $"I{model.Name}Service.cs", text);
        });
    }
}