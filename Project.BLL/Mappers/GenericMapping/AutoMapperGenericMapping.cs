using AutoMapper;

namespace Project.BLL.Mappers.GenericMapping;

public class AutoMapperGenericMapping<TProfile> : IGenericMapper where TProfile : Profile, new()
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        var mapper = new MapperConfiguration(config => { config.AddProfile(new TProfile()); }).CreateMapper();

        return mapper.Map<TDestination>(source);
    }
}