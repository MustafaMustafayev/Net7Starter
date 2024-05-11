using API.Containers;
using API.Filters;
using API.Graphql.Role;
using API.Hubs;
using API.Middlewares;
using API.Services;
using BLL.Mappers;
using CORE.Config;
using CORE.Constants;
using DAL.EntityFramework.Context;
using DTO;
using FluentValidation;
using FluentValidation.AspNetCore;
using GraphQL.Server.Ui.Voyager;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Text.Json.Serialization;
using WatchDog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddWatchDogLogger();

builder.Services.RegisterLogger();

builder.Services.RegisterWatchDog();

var config = new ConfigSettings();

builder.Configuration.GetSection(nameof(ConfigSettings)).Bind(config);

builder.Services.TryAddSingleton(config);

builder.Services.AddControllers(opt => opt.Filters.Add(typeof(ModelValidatorActionFilter)))
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<DtoObject>();

if (config.SentrySettings.IsEnabled)
{
    builder.WebHost.UseSentry();
}

builder.Services.AddAutoMapper(Automapper.GetAutoMapperProfilesFromAllAssemblies().ToArray());

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(config.ConnectionStrings.AppDb));

builder.Services.AddHttpContextAccessor();

builder.Services.RegisterRefitClients(config);

if (config.RedisSettings.IsEnabled)
{
    builder.Services.AddHostedService<RedisIndexCreatorService>();
    builder.Services.RegisterRedis(config);
}

if (config.ElasticSearchSettings.IsEnabled)
{
    builder.Services.RegisterElasticSearch(config);
}

if (config.MongoDbSettings.IsEnabled)
{
    builder.Services.RegisterMongoDb();
}

// configure max request body size as 60 MB
builder.Services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = 60 * 1024 * 1024);

builder.Services.RegisterRepositories();
builder.Services.RegisterSignalRHubs();
builder.Services.RegisterUnitOfWork();
builder.Services.RegisterApiVersioning();
builder.Services.RegisterRateLimit();
builder.Services.RegisterOutputCache();
builder.Services.RegisterMediatr();

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddSorting()
    .AddFiltering();

builder.Services.AddHealthChecks().AddNpgSql(config.ConnectionStrings.AppDb);

builder.Services.RegisterAuthentication(config);

builder.Services.AddCors(o => o
    .AddPolicy(Constants.EnableAllCorsName, b => b
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()));

builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddScoped<ModelValidatorActionFilter>();

builder.Services.AddEndpointsApiExplorer();

if (config.SwaggerSettings.IsEnabled)
{
    builder.Services.RegisterSwagger(config);
}

builder.Services.RegisterMiniProfiler();

builder.Services.AddSignalR();

//builder.Services.AddAntiforgery();

var app = builder.Build();

// app.UseAntiforgery();

// if (app.Environment.IsDevelopment())

if (config.SwaggerSettings.IsEnabled)
{
    app.UseSwagger();
}

if (config.SwaggerSettings.IsEnabled)
{
    app.UseSwaggerUI(c =>
    {
        c.EnablePersistAuthorization();
        c.InjectStylesheet(config.SwaggerSettings.Theme);
    });
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseCors(Constants.EnableAllCorsName);

app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseOutputCache();
app.UseHttpsRedirection();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

// this will cause unexpected behaviour on watchdog's site
/*app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Frame-Options", "Deny");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    await next.Invoke();
});*/

if (config.SentrySettings.IsEnabled)
{
    app.UseSentryTracing();
}

app.UseStaticFiles();

app.UseAuthorization();

app.UseAuthentication();

// app.UseMiniProfiler();

app.UseRateLimiter();

app.MapHealthChecks(
    "/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.MapControllers();

app.MapHub<UserHub>("/userHub");

app.MapGraphQL((PathString)"/graphql");

app.UseGraphQLVoyager("/graphql-voyager", new VoyagerOptions
{
    GraphQLEndPoint = "/graphql"
});

app.UseWatchDogExceptionLogger();

app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "admin";
    //Optional
    //opt.Blacklist = "Test/testPost, api/auth/login"; //Prevent logging for specified endpoints
    //opt.Serializer = WatchDogSerializerEnum.Newtonsoft; //If your project use a global json converter
    //opt.CorsPolicy = "MyCorsPolicy";
});

app.Run();