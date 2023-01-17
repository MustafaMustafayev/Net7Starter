using SourceBuilder.Builders.Abstract;
using SourceBuilder.Helpers;
using SourceBuilder.Models;

namespace SourceBuilder.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IUnitOfWorkBuilder : IBuilder
{
    public void Build(List<Entity> entities)
    {
        var text = TextBuilder.BuildTextForIUnitOfWork(entities);
        Workers.SourceBuilder.Instance.AddSourceFile(Constants.IUnitOfWorkPath, "IUnitOfWork.cs", text);
    }
}