using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;
using System.Text;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class UnitOfWorkBuilder : ISourceBuilder
{
    private readonly string ProjectPath;
    private readonly string RootNamespace = "DAL.EntityFramework.UnitOfWork";
    private readonly string DefaultDocumentBody = @"using DAL.EntityFramework.Abstract;
using DAL.EntityFramework.Context;

namespace DAL.EntityFramework.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    private bool _isDisposed;

    public UnitOfWork(DataContext dataContext) {
        _dataContext = dataContext;
    }

    public async Task CommitAsync()
    {
        await _dataContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;
        _isDisposed = true;
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            _dataContext.Dispose();
    }

    private async ValueTask DisposeAsync(bool disposing)
    {
        if (disposing)
            await _dataContext.DisposeAsync();
    }
}";

    public UnitOfWorkBuilder()
    {
        ProjectPath = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.ToString(),
            @"DAL\DAL.csproj");
    }

    public void BuildSourceFile(List<Entity> entities)
    {
        var result = GenerateSource(entities.Where(w =>
                w.Options.BuildUnitOfWork
                && w.Options.BuildRepository)
            .ToList()).Result;
    }

    private async Task<string> GenerateSource(List<Entity> entities)
    {
        using MSBuildWorkspace workspace = MSBuildWorkspace.Create();
        workspace.WorkspaceFailed += (source, args)=>
        {
            if(args.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
            {
                Console.WriteLine(args.Diagnostic.Message);
            }
        };

        Project project = await workspace.OpenProjectAsync(ProjectPath);
        Document document = project.Documents.Where(w=>w.Name=="UnitOfWork.cs").FirstOrDefault();

        if(document != null && !entities.Any()) {
            return string.Empty;
        }

        if (document is null)
        {
            document = project
                .AddDocument("UnitOfWork.cs", DefaultDocumentBody, ["EntityFramework", "UnitOfWork"]);
        }

        SyntaxTree? syntaxTree = await document.GetSyntaxTreeAsync();
        SyntaxNode syntaxNode = await syntaxTree.GetRootAsync();
        ClassDeclarationSyntax classDeclaration = syntaxNode.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();

        ConstructorDeclarationSyntax constructor = classDeclaration.DescendantNodes().OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
        List<PropertyDeclarationSyntax> properties = new List<PropertyDeclarationSyntax>();
        List<ParameterSyntax> parameters = new List<ParameterSyntax>();
        
        foreach (Entity entity in entities)
        {
            var existEntity = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>()
                .Any(w => w.Identifier.Text == $"{entity.Name}Repository");
            if (existEntity) continue;
            properties.Add(BuildProperty(classDeclaration, entity));

            constructor = BuildConstructor(constructor, entity);
        }

        classDeclaration = classDeclaration.ReplaceNode(classDeclaration.DescendantNodes().OfType<ConstructorDeclarationSyntax>().FirstOrDefault(), constructor);

        if (properties.Count > 0)
        {
            classDeclaration = classDeclaration.AddMembers(properties.ToArray());
            syntaxNode = syntaxNode.ReplaceNode(syntaxNode.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault(), classDeclaration);
        }        

        Document newDocument = document.WithSyntaxRoot(syntaxNode.NormalizeWhitespace());
        
        workspace.TryApplyChanges(newDocument.Project.Solution);
        
        
        return string.Empty;
    }

    private static ConstructorDeclarationSyntax BuildConstructor(ConstructorDeclarationSyntax constructor, Entity entity)
    {
        ParameterSyntax newParameter = SyntaxFactory
            .Parameter(SyntaxFactory.Identifier($"{entity.Name.FirstCharToLowerCase()}Repository"))
            .WithType(SyntaxFactory.ParseTypeName($"I{entity.Name}Repository"));

        StatementSyntax newStatment = SyntaxFactory
            .ParseStatement($"{entity.Name}Repository = {entity.Name.FirstCharToLowerCase()}Repository;");

        ParameterListSyntax newParameterList = constructor.ParameterList.AddParameters(newParameter);
        constructor = constructor.WithParameterList(newParameterList).AddBodyStatements(newStatment);
        return constructor;
    }

    private static PropertyDeclarationSyntax BuildProperty(ClassDeclarationSyntax? classDeclaration, Entity entity)
    {
        var property = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName($"I{entity.Name}Repository"), $"{entity.Name}Repository")
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddAccessorListAccessors(
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        )
                        .WithTrailingTrivia(SyntaxFactory.Space);
        return property.NormalizeWhitespace();
    }



    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        throw new NotImplementedException();
    }

}