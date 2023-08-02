using DTO.Responses;
using DTO.Role;
using NBomber.CSharp;

namespace LoadTesting;

public class RoleTest : RootTest
{
    public Task RunList()
    {
        var scenario = Scenario.Create("http_role_list_test", async context =>
            {
                var step = await CreateHttpStep<SuccessDataResult<List<RoleToListDto>>>(
                    "fist_step",
                    context,
                    HttpMethod.Get,
                    "api/Role",
                    string.Empty
                );

                return Response.Ok();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.KeepConstant(3, TimeSpan.FromSeconds(30))
            );
        var result = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
        return Task.CompletedTask;
    }

    public Task RunById()
    {
        var scenario = Scenario.Create("http_role_by_id_test", async context =>
            {
                var step = await CreateHttpStep<SuccessDataResult<RoleToListDto>>(
                    "second_step",
                    context,
                    HttpMethod.Get,
                    "api/Role/1",
                    string.Empty
                );
                return Response.Ok();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(
                Simulation.Inject(1, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(30))
            );
        var result = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
        return Task.CompletedTask;
    }
}