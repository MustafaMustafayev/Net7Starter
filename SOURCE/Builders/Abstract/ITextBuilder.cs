using SOURCE.Models;

namespace SOURCE.Builders.Abstract;

public interface ITextBuilder
{
    public string BuildSourceText(Entity? entity, List<Entity>? entities);
}