using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using SOURCE.Builders.Abstract;
using SOURCE.Models;
using SOURCE.Workers;
using System.Reflection;
using System.Text;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class DtosBuilder : ISourceBuilder
{
    private readonly string EntityProjectPath;
    private readonly string RootNamespace = "ENTITIES.Entities";

    public DtosBuilder()
    {
        EntityProjectPath = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.ToString(),
            @"ENTITIES\ENTITIES.csproj");
    }
    public void BuildSourceFile(List<Entity> entities)
    {
        //entities.ForEach(model => SourceBuilder.Instance
        //    .AddSourceFile(Constants.DtoPath.Replace("{entityName}", model.Name),
        //        $"{model.Name}Dtos.cs", BuildSourceText(model, null)));
        entities
            .Where(w=>w.Options.BuildDto)
            .ToList()
            .ForEach(entity =>
            {
                string properties = GetProperties(entity).Result;
                SourceBuilder.Instance.AddSourceFile(
                    Constants.DtoPath.Replace("{entityName}", entity.Name),
                    $"{entity.Name}CreateRequestDto.cs", GenerateContent(entity.Name, $"{entity.Name}CreateRequestDto", properties));

                SourceBuilder.Instance.AddSourceFile(
                    Constants.DtoPath.Replace("{entityName}", entity.Name),
                    $"{entity.Name}UpdateRequestDto.cs", GenerateContent(entity.Name, $"{entity.Name}UpdateRequestDto", properties));

                SourceBuilder.Instance.AddSourceFile(
                    Constants.DtoPath.Replace("{entityName}", entity.Name),
                    $"{entity.Name}ResponseDto.cs", GenerateContent(entity.Name, $"{entity.Name}ResponseDto", properties));

                SourceBuilder.Instance.AddSourceFile(
                    Constants.DtoPath.Replace("{entityName}", entity.Name),
                    $"{entity.Name}ByIdResponseDto.cs", GenerateContent(entity.Name, $"{entity.Name}ByIdResponseDto", properties));
            });
    }

    private async Task<string> GetProperties(Entity entity)
    {
        string fullNamespace = RootNamespace + (!string.IsNullOrEmpty(entity!.Path) ? $".{entity.Path}" : string.Empty);
        StringBuilder result = new StringBuilder();
        //if (!MSBuildLocator.IsRegistered)
        //{
        //    var instances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
        //    MSBuildLocator.RegisterInstance(instances.OrderByDescending(x => x.Version).First());
        //}
        using (MSBuildWorkspace workspace = MSBuildWorkspace.Create())
        {
            workspace.WorkspaceFailed += (source, args) =>
            {
                if (args.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
                {
                    Console.Error.WriteLine(args.Diagnostic.Message);
                }
            };
            Project project = await workspace.OpenProjectAsync(EntityProjectPath);
            Compilation? compilation = await project.GetCompilationAsync();

            if (compilation is null) return result.ToString();

            INamedTypeSymbol? classSymbol = compilation.GetTypeByMetadataName(fullNamespace + "." + entity.Name);
            if (classSymbol is null) return result.ToString();

            GetClassProperties(result, classSymbol);

            if(
                classSymbol.BaseType.ContainingNamespace.ToDisplayString().StartsWith("ENTITIES")
                && !classSymbol.BaseType.Name.Contains("Auditable"))
            {
                GetClassProperties(result, classSymbol.BaseType);
            }
        }
        return result.ToString();
    }

    private static void GetClassProperties(StringBuilder result, INamedTypeSymbol? classSymbol)
    {
        List<ISymbol> properties = classSymbol.GetMembers().Where(w => w.Kind == SymbolKind.Property).ToList();

        foreach (var property in properties)
        {
            INamedTypeSymbol propertyType = ((INamedTypeSymbol)((IPropertySymbol)property).Type);
            if (propertyType.IsValueType)
            {
                result.AppendLine($"\tpublic {propertyType.ToDisplayString()} {property.Name} {{get; set;}}");
            }
            else if (propertyType.Name == "String")
            {
                result.AppendLine($"\tpublic string {property.Name} {{get; set;}}");
            }
            else if (propertyType.IsGenericType)
            {
                if (propertyType.TypeArguments[0].IsValueType)
                {
                    result.AppendLine($"\tpublic {propertyType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} {property.Name} {{get; set;}}");
                }
                else
                {
                    result.AppendLine($"\tpublic {propertyType.Name}<object> {property.Name} {{get; set;}}");
                }
            }
            else
            {
                result.AppendLine($"\tpublic object {property.Name} {{get; set;}}");
            }
        }
    }

    private string GenerateContent(string name, string className, string properties)
    {
        var text = """
                   namespace DTO.{0};

                   public record {1}
                   {2}
                   {4}
                   {3}

                   """;

        return string.Format(text, name, className, "{", "}", properties);
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var text = """
                   namespace DTO.{entityName};

                   public record {entityName}CreateRequestDto();
                   public record {entityName}UpdateRequestDto();
                   public record {entityName}ResponseDto(Guid Id);
                   public record {entityName}ByIdResponseDto(Guid Id);

                   """;

        text = text.Replace("{entityName}", entity!.Name);
        return text;
    }

}