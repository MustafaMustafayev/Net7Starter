using SourceBuilder.Models;

namespace SourceBuilder.Builders.Abstract;

public interface IBuilder
{
    void Build(List<Entity> entities);
}