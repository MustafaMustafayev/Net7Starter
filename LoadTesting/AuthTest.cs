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

namespace LoadTesting;

public class AuthTest : RootTest
{
    public void RunLogin()
    {
        var scenario = Scenario.Create("http_login_test", async context =>
        {
            var request = CreateRequest("POST", "api/Auth/login", "{\r\n  \"email\": \"test@test.tst\",\r\n  \"password\": \"testtest\"\r\n}");

            var response = await Http.Send(httpClient, request);
            if (response.IsError)
            {
                return Response.Fail(message: response.Message, statusCode: response.StatusCode);
            }
            string result = await response.Payload.Value.Content.ReadAsStringAsync();

            var setting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            SuccessDataResult<LoginResponseDto?>? token = JsonConvert.DeserializeObject<SuccessDataResult<LoginResponseDto?>>(result, setting);

            if (token is null || token.Data is null || token.Data.User is null || token.Data.User.Email!="test@test.tst")
            {
                return Response.Fail(message: token.Message);
            }

            return Response.Ok();
        })
        .WithoutWarmUp()
        .WithLoadSimulations(
            Simulation.Inject(rate: 10, interval: TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(30))
            );
        var result = NBomberRunner
             .RegisterScenarios(scenario)
             .Run();
    }


}
