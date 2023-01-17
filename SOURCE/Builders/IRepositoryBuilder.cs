using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IRepositoryBuilder : IBuilder
{
    public void BuildSourceCode(List<Entity> entities)
    {
        entities.ForEach(model =>
        {
            var text = TextBuilder.BuildTextForIRepository(model);
            SourceBuilder.Instance.AddSourceFile(Constants.IRepositoryPath, $"I{model.Name}Repository.cs", text);
        });
    }
}