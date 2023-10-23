using DTO.Auth;
using DTO.Responses;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace LoadTesting;

public abstract class RootTest
{
    private const string BaseUrl = "https://localhost:7086/";
    private readonly HttpClient _httpClient;
    private string? _accessToken;
    private bool _isAuthenticate;
    private string? _refreshToken;

    protected RootTest()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BaseUrl);

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpClient.DefaultProxy = new WebProxy
        {
            Credentials = CredentialCache.DefaultCredentials
        };

        Login().Wait();
    }

    private async Task Login()
    {
        var login = new LoginDto("test@test.tst", "testtest");
        using var httpResponse = await _httpClient.PostAsJsonAsync(new Uri(BaseUrl + "api/Auth/login"), login);

        var result = await httpResponse.Content.ReadAsStringAsync();

        var setting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        var token = JsonConvert.DeserializeObject<SuccessDataResult<LoginResponseDto?>>(result, setting);

        if (token?.Data?.AccessToken is null || !token.Success) throw new UnauthorizedAccessException();

        _isAuthenticate = true;
        _accessToken = token.Data.AccessToken;
        _refreshToken = token.Data.RefreshToken;
    }

    private HttpRequestMessage CreateRequest(string type, string url, string? jsonBody)
    {
        var request = Http.CreateRequest(type, BaseUrl + url)
            .WithHeader("Content-Type", "application/json")
            .WithHeader("Accept", "application/json");

        if (_isAuthenticate)
        {
            request.WithHeader("Authorization", "Bearer " + _accessToken);
            request.WithHeader("RefreshToken", _refreshToken);
        }

        if (jsonBody is not null) request.WithBody(new StringContent(jsonBody, Encoding.UTF8, "application/json"));

        return request;
    }

    /// <summary>
    ///     Helper method for create step.
    /// </summary>
    /// <typeparam name="TResponse">Type used for deserialize object/</typeparam>
    /// <param name="stepName">Step name</param>
    /// <param name="context">Scenario context</param>
    /// <param name="httpMethod">Http method</param>
    /// <param name="url">Url to API</param>
    /// <param name="payload">Payload for API</param>
    /// <param name="validator">Method for validation data</param>
    /// <returns></returns>
    protected async Task<Response<object>> CreateHttpStep<TResponse>(
        string stepName,
        IScenarioContext context,
        HttpMethod httpMethod,
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
                var request = CreateRequest(httpMethod.ToString(), url, payload);
                var response = await Http.Send(_httpClient, request);

                if (response.IsError) return Response.Fail(message: response.Message, statusCode: response.StatusCode);

                var result = await response.Payload.Value.Content.ReadAsStringAsync();

                var setting = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                };

                if (validator is not null)
                {
                    var data = JsonConvert.DeserializeObject<TResponse>(result, setting);

                    if (data is null)
                    {
                        context.Logger.Error("Faild deseroalized object");
                        return Response.Fail();
                    }

                    if (!validator(context, data)) return Response.Fail(message: "Data is invalid");
                }
            }
            catch (Exception ex)
            {
                context.Logger.Error(ex.ToString());
                return Response.Fail();
            }

            return Response.Ok();
        });
    }
}