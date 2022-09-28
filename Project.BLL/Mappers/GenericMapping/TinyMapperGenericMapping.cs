using Nelibur.ObjectMapper;

namespace Project.BLL.Mappers.GenericMapping;

public class TinyMapperGenericMapping : IGenericMapper
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        TinyMapper.Bind<TSource, TDestination>();
        return TinyMapper.Map<TDestination>(source);
    }
}