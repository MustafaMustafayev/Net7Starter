using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class AutomapperBuilder : IBuilder
{
    public void BuildSourceCode(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForAutomapper(model);
            SourceBuilder.Instance.AddSourceFile(Constants.AutomapperPath, $"{model.Name}Mapper.cs", text);
        });
    }
}