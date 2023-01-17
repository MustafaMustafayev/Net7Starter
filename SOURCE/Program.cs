Console.WriteLine("Hello, World!");
Console.WriteLine("I am starting to build, plase wait..");
Console.WriteLine("I am not stuck. I working hard on millions of lines please be patient..");

var sourceBuilder = SourceBuilder.Workers.SourceBuilder.Instance;
Console.WriteLine(
    await sourceBuilder.BuildSourceFiles()
        ? "I am generated all of your code."
        : "Error happened during process!"
);