using System.Net;
using System;
using NBomber.Http.CSharp;
using System.Text;
using System.Net.Http.Json;
using DTO.Auth;
using DTO.Responses;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Text.Json.Serialization;
using NBomber.CSharp;
using DTO.Role;
using FluentValidation.Internal;

namespace LoadTesting;

public class RoleTest : RootTest
{
    public async Task RunList()
    {
        await Login();
        var scenario = Scenario.Create("http_role_list_test", async context =>
        {
            var request = CreateRequest("GET", "api/Role",String.Empty);
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

            var data = JsonConvert.DeserializeObject<SuccessDataResult<List<RoleToListDto>>>(result, setting);

            if (data is null || data.Data is null || data.Data.Count == 0)
                return Response.Fail();

            return Response.Ok();
        })
         .WithoutWarmUp()
         .WithLoadSimulations(
             Simulation.RampingConstant(copies:10,during: TimeSpan.FromMinutes(1))
             );
        var result = NBomberRunner
             .RegisterScenarios(scenario)
             .Run();
    }

    public async Task RunById()
    {
        await Login();
        var scenario = Scenario.Create("http_role_by_id_test", async context =>
        {
            var request = CreateRequest("GET", "api/Role/1", String.Empty);
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

            var data = JsonConvert.DeserializeObject<SuccessDataResult<RoleToListDto>>(result, setting);

            if (data is null || data.Data is null) return Response.Fail();

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

