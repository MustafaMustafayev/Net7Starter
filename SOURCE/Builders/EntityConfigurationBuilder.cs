using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class EntityConfigurationBuilder : ISourceBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        entities
            .Where(w => w.Configure.HasValue && w.Configure.Value)
            .ToList()
            .ForEach(model =>
            {
                SourceBuilder.Instance.AddSourceFile(Constants.ENTITY_CONFIGURATION_PATH, $"{model.Name}Configuration.cs",
                    BuildSourceText(model, null));

            });
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   using ENTITIES.Entities{entityPath};
                   using Microsoft.EntityFrameworkCore;
                   using Microsoft.EntityFrameworkCore.Metadata.Builders;

                   namespace DAL.EntityFramework.Configurations;

                   public class {entityName}Configuration : IEntityTypeConfiguration<{entityName}>
                   {
                       public void Configure(EntityTypeBuilder<{entityName}> builder)
                       {
                           
                       }
                   }
                   """;
        text = text.Replace("{entityName}", entity!.Name);
        text = text.Replace("{entityPath}", !string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        return text;
    }
}