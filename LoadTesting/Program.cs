// See https://aka.ms/new-console-template for more information
using LoadTesting;

Task.Run(async() =>
{
    await new RoleTest().RunById();
});

Console.Read();