using SOURCE.Builders.Abstract;
using SOURCE.Helpers;
using SOURCE.Models;

namespace SOURCE.Workers;

public class SourceBuilder
{
    private static SourceBuilder? _instance;
    private static readonly object Padlock = new();

    private readonly List<SourceFile> _sourceFiles = new();

    private SourceBuilder()
    {
    }

    public static SourceBuilder Instance
    {
        get
        {
            lock (Padlock)
            {
                if (_instance == null)
                {
                    _instance = new SourceBuilder();
                }

                return _instance;
            }
        }
    }

    public void AddSourceFile(string filePath, string fileName, string text, bool skipIfExists = true)
    {
        _sourceFiles.Add(new SourceFile { Path = filePath, Name = fileName, Text = text, SkipIfExists = skipIfExists });
    }

    private async Task<bool> CreateAllSourceFilesAsync()
    {
        if (!_sourceFiles.Any())
        {
            return true;
        }

        foreach (var sourceFile in _sourceFiles)
        {
            if (!await FileHelper.CreateFileAsync(sourceFile))
            {
                return false;
            }
        }

        return true;
    }

    private static void AddAllSourceFilesAsync(List<Entity> entities)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(ISourceBuilder).IsAssignableFrom(p) && p != typeof(ISourceBuilder)).ToList();

        foreach (var instance in types.Select(type => (ISourceBuilder)Activator.CreateInstance(type)!))
        {
            instance.BuildSourceFile(entities);
        }
    }

    public async Task<bool> BuildSourceFiles()
    {
        var entities = await FileHelper.ReadJsonAsync();

        AddAllSourceFilesAsync(entities);
        return await CreateAllSourceFilesAsync();
    }
}