using DTO.Auth;
using DTO.Responses;
using NBomber.Contracts;
using NBomber.Contracts.Stats;
using NBomber.CSharp;

using NBomber.Http.CSharp;
using Nest;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Formats.Asn1;
using System.Net;
using System.Text;
using DTO.Role;

namespace LoadTesting;

public class AuthTest : RootTest
{
    public async Task RunLogin()
    {
        var scenario = Scenario.Create("http_login_test", async context =>
        {
            var step = await CreateHttpStep<SuccessDataResult<LoginResponseDto>>(
                "login_step",
                context,
                HttpMethods.POST,
                "api/Auth/login",
                "{\r\n  \"email\": \"test@test.tst\",\r\n  \"password\": \"testtest\"\r\n}",
                validator: (context, data)=> data.Success
            );

            return Response.Ok();
        })
        .WithoutWarmUp()
         .WithLoadSimulations(
             Simulation.KeepConstant(copies: 1, during: TimeSpan.FromSeconds(30))
             );
        var result = NBomberRunner
             .RegisterScenarios(scenario)
             .Run();
    }


}
