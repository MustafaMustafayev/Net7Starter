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
}
