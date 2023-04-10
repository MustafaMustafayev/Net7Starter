using DTO.Auth;
using DTO.Responses;
using NBomber.CSharp;

namespace LoadTesting;

public class AuthTest : RootTest
{
    public void RunLogin()
    {
        var scenario = Scenario.Create("http_login_test", async context =>
            {
                var step = await CreateHttpStep<SuccessDataResult<LoginResponseDto>>(
                    "login_step",
                    context,
                    HttpMethod.Post,
                    "api/Auth/login",
                    "{'email': 'test@test.tst','password': 'testtest'}",
                    (context, data) => data.Success
                );

                return Response.Ok();
            })
            .WithoutWarmUp()
            .WithLoadSimulations(Simulation.KeepConstant(1, TimeSpan.FromMinutes(1)));

        var result = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();
    }
}