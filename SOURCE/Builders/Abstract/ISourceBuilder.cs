using SOURCE.Models;

namespace SOURCE.Builders.Abstract;

public interface ISourceBuilder
{
    public string BuildSourceText(Entity? entity, List<Entity>? entities);
    public void BuildSourceFile(List<Entity> entities);
}