using SOURCE;
using SOURCE.Workers;

Console.WriteLine("Hello, World!");
Console.WriteLine("Please tell me, what you want to build?");
Console.WriteLine("1 - Source Codes For Entity");
Console.WriteLine("2 - Clients For Refit");

int typeOfBuild;

if (!int.TryParse(Console.ReadLine(), out typeOfBuild))
    Console.WriteLine("Pick valid number!");

if (typeOfBuild == 1)
    await EntityBuilderDialogAsync();

if (typeOfBuild == 2)
    await ClientBuilderDialogAsync();


async Task EntityBuilderDialogAsync()
{
    Console.WriteLine("I am starting to build, plase wait..");
    Console.WriteLine("I am not stuck. I working hard on millions of lines please be patient..");

    var sourceBuilder = SourceBuilder.Instance;
    Console.WriteLine(
        await sourceBuilder.BuildSourceFiles()
            ? "I generated all of your code."
            : "Error has happened during process!"
    );
}

async Task ClientBuilderDialogAsync()
{
    Console.WriteLine("If you have url of API which you want to create enter. Othervise press 'enter'.");
    Console.WriteLine("For example https://my.api.user/swagger/v1/swagger.json");

    var openApiJsonUrl = Console.ReadLine();

    Console.WriteLine("I am starting to build, plase wait..");
    Console.WriteLine("I am not stuck. I working hard on millions of lines please be patient..");

    if (string.IsNullOrEmpty(openApiJsonUrl?.Trim())) openApiJsonUrl = Constants.OpenApiFileName;

    RefitBuilder refitBuilder = new RefitBuilder();
    // Relative or absolute path to .json or .yaml local file or a URL to a .json or .yaml file
    var generatedCode = await refitBuilder.BuildClientsAsync(openApiJsonUrl);

    Console.WriteLine("I have generated all of your code.");
    Console.WriteLine(generatedCode);
}