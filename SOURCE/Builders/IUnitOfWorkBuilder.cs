using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class IUnitOfWorkBuilder : IBuilder
{
    public void BuildSourceCode(List<Entity> entities)
    {
        var text = TextBuilder.BuildTextForIUnitOfWork(entities);
        SourceBuilder.Instance.AddSourceFile(Constants.IUnitOfWorkPath, "IUnitOfWork.cs", text);
    }
}