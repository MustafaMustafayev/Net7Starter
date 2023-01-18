using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class UnitOfWorkBuilder : IBuilder
{
    public void BuildSourceCode(List<Entity> entities)
    {
        var text = TextBuilder.BuildTextForUnitOfWork(entities);
        SourceBuilder.Instance.AddSourceFile(Constants.UnitOfWorkPath, "UnitOfWork.cs", text);
    }
}