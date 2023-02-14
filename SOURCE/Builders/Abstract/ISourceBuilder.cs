using SOURCE.Models;

namespace SOURCE.Builders.Abstract;

public interface ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities);
}