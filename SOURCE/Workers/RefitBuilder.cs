using Refitter.Core;

namespace SOURCE.Workers;

public class RefitBuilder
{
    public async Task<string> BuildClientsAsync(string openApiJsonOrYamlPath)
    {
        var settings = new RefitGeneratorSettings
        {
            OpenApiPath = openApiJsonOrYamlPath
        };

        var generator = await RefitGenerator.CreateAsync(settings);

        var generatedCode = generator.Generate();

        return generatedCode;
    }
}