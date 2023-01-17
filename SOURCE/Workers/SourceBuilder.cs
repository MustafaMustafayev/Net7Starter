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
                if (_instance == null) _instance = new SourceBuilder();
                return _instance;
            }
        }
    }

    public void AddSourceFile(string filePath, string fileName, string text)
    {
        _sourceFiles.Add(new SourceFile { Path = filePath, Name = fileName, Text = text });
    }

    public async Task<bool> BuildSourceFiles()
    {
        var entities = await FileHelper.ReadJsonAsync();

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes()).Where(p => typeof(IBuilder).IsAssignableFrom(p) && p != typeof(IBuilder)).ToList();

        foreach (var type in types)
        {
            var instance = (IBuilder)Activator.CreateInstance(type)!;
            instance.BuildSourceCode(entities);
        }

        if (!_sourceFiles.Any()) return true;

        foreach (var sourceFile in _sourceFiles)
            if (!await FileHelper.CreateFileAsync(sourceFile))
                return false;


        return true;
    }
}