﻿using System.Text;
using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;
using SOURCE.Workers;

namespace SOURCE.Builders;

// ReSharper disable once UnusedType.Global
public class UnitOfWorkBuilder : ISourceBuilder, ITextBuilder
{
    public void BuildSourceFile(List<Entity> entities)
    {
        SourceBuilder.Instance.AddSourceFile(Constants.UnitOfWorkPath, "UnitOfWork.cs",
            BuildSourceText(null, entities));
    }

    public string BuildSourceText(Entity? entity, List<Entity>? entities)
    {
        var constructorArguments = new StringBuilder();
        entities!.ForEach(e =>
        {
            if (entities.Last() == e)
                constructorArguments.Append(
                    $"        I{e.Name}Repository {e.Name.FirstCharToLowerCase()}Repository");
            else
                constructorArguments.AppendLine(
                    $"        I{e.Name}Repository {e.Name.FirstCharToLowerCase()}Repository,");
        });

        var constructorSetters = new StringBuilder();
        entities.ForEach(e =>
        {
            if (entities.Last() == e)
                constructorSetters.Append(
                    $"        {e.Name}Repository = {e.Name.FirstCharToLowerCase()}Repository;");
            else
                constructorSetters.AppendLine(
                    $"        {e.Name}Repository = {e.Name.FirstCharToLowerCase()}Repository;");
        });

        var properties = new StringBuilder();
        entities.ForEach(e =>
            properties.AppendLine($"    public I{e.Name}Repository {e.Name}Repository {{ get; set; }}"));


        var text = $@"
using DAL.Abstract;
using DAL.DatabaseContext;
using DAL.UnitOfWorks.Abstract;

namespace DAL.UnitOfWorks.Concrete;

public class UnitOfWork : IUnitOfWork
{{
    private readonly DataContext _dataContext;

    private bool _isDisposed;

    public UnitOfWork(
        DataContext dataContext,
{constructorArguments}
    )
    {{
        _dataContext = dataContext;
{constructorSetters}
    }}

{properties}
    public async Task CommitAsync()
    {{
        await _dataContext.SaveChangesAsync();
    }}

    public async ValueTask DisposeAsync()
    {{
        if (!_isDisposed)
        {{
            _isDisposed = true;
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }}
    }}

    public void Dispose()
    {{
        if (_isDisposed) return;
        _isDisposed = true;
        Dispose(true);
        GC.SuppressFinalize(this);
    }}

    protected virtual void Dispose(bool disposing)
    {{
        if (disposing) _dataContext.Dispose();
    }}

    private async ValueTask DisposeAsync(bool disposing)
    {{
        if (disposing) await _dataContext.DisposeAsync();
    }}
}}
";

        return text;
    }
}