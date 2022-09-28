using Mapster;

namespace Project.BLL.Mappers.GenericMapping;

public class MapsterGenericMapping : IGenericMapper
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TDestination>();
    }
}