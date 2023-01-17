using SOURCE.Models;

namespace SOURCE.Builders.Abstract;

public interface IBuilder
{
    void BuildSourceCode(List<Entity> entities);
}