using DTO.Auth;
using DTO.Responses;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Nest;
using System.Text.Json.Nodes;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LoadTesting;

public abstract class RootTest
{
    protected readonly string BaseUrl = "https://localhost:7086/";
    protected readonly HttpClient httpClient;
    protected bool IsAuthenticate = false;
    protected string AccessToken = "";
    protected string RefreshToken = "";
    private string login = "test@test.tst";
    private string password = "testtest";
    public enum HttpMethods
    {
        GET,
        POST,
        PUT,
        PATCH,
        DELETE
    }


    public RootTest()
    {
        httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(BaseUrl);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        HttpClient.DefaultProxy = new WebProxy()
        {
            Credentials = CredentialCache.DefaultCredentials
        };
    }

    public async Task Login()
    {
        var login = new LoginDto() { Email = "test@test.tst", Password = "testtest" };
        using HttpResponseMessage httpResponse = await httpClient.PostAsJsonAsync(new Uri(BaseUrl + "api/Auth/login"), login);
     
        string result = await httpResponse.Content.ReadAsStringAsync();

        var setting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };
        SuccessDataResult<LoginResponseDto?>? token = JsonConvert.DeserializeObject<SuccessDataResult<LoginResponseDto?>>(result, setting);

        if (token is null || token.Data is null) throw new UnauthorizedAccessException();

        if (token.Success && token.Data.AccessToken is not null)
        {
            IsAuthenticate = true;
            AccessToken = token.Data.AccessToken;
            RefreshToken = token.Data.RefreshToken;
        }

        return;
    }

    public HttpRequestMessage CreateRequest(string type, string url, string jsonBody)
    {
        var request = Http.CreateRequest(type, BaseUrl + url)
              .WithHeader("Content-Type", "application/json")
              .WithHeader("Accept", "application/json");

        if (IsAuthenticate)
        {
            request = request.WithHeader("Authorization", "Bearer " + AccessToken);
            request = request.WithHeader("RefreshToken", RefreshToken);
        }

        if (jsonBody is not null)
        {
            request = request.WithBody(new StringContent(jsonBody, Encoding.UTF8, "application/json"));
        }

        return request;
    }

    /// <summary>
    /// Helper method for create step.
    /// </summary>
    /// <typeparam name="TResponse">Type used for deserialize object/</typeparam>
    /// <param name="stepName">Step name</param>
    /// <param name="context">Scenario context</param>
    /// <param name="httpMethods">Http method</param>
    /// <param name="url">Url to API</param>
    /// <param name="payload">Payload for API</param>
    /// <param name="validator">Method for validation data</param>
    /// <returns></returns>
    public async Task<Response<object>> CreateHttpStep<TResponse>(
        string stepName,
        IScenarioContext context,
        HttpMethods httpMethods,
        string url,
        string payload,
        Func<IScenarioContext, TResponse, bool>? validator = null
        )
        where TResponse : class
    {
        return await Step.Run("step_1", context, async () =>
        {
            try
            {
                var request = CreateRequest(httpMethods.ToString(), url, payload);
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

                if (validator is not null)
                {
                    TResponse? data = JsonConvert.DeserializeObject<TResponse>(result, setting);

                    if (data is null)
                    {
                        context.Logger.Error("Faild deseroalized object");
                        return Response.Fail();
                    }

                    if (!validator(context, data))
                    {
                        return Response.Fail(message: "Data is invalid");
                    }
                }
            } catch(Exception ex)
            {
                context.Logger.Error(ex.Message);
                return Response.Fail();
            }

            return Response.Ok();
        });
    }
}
