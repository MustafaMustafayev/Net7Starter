namespace Project.BLL.Mappers.GenericMapping;

public interface IGenericMapper
{
    TDestination Map<TSource, TDestination>(TSource source);
}