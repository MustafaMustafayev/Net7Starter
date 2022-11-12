using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Project.API.ActionFilters;
using Project.API.Containers;
using Project.API.Hubs;
using Project.API.Services;
using Project.BLL.Mappers;
using Project.BLL.MediatR;
using Project.Core.Config;
using Project.Core.Constants;
using Project.Core.Middlewares.ExceptionHandler;
using Project.Core.Middlewares.Translation;
using Project.DAL.DatabaseContext;
using Project.DTO.Auth.AuthValidators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterNLogger();

var config = new ConfigSettings();

builder.Configuration.GetSection("Config").Bind(config);

builder.Services.AddSingleton(config);

builder.Services.AddControllers(opt => opt.Filters.Add(typeof(ValidatorActionFilter)));

builder.Services.AddFluentValidationAutoValidation().AddValidatorsFromAssemblyContaining<LoginDtoValidator>();

if (config.SentrySettings.IsEnabled) builder.WebHost.UseSentry();

builder.Services.AddAutoMapper(Automapper.GetAutoMapperProfilesFromAllAssemblies().ToArray());

// moved to DAL, to remove Microsoft.EntityFrameworkCore.Design dependency from API layer
builder.Services.AddDatabaseContext(config.ConnectionStrings.AppDb);

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services.RegisterHttpClients(config);

builder.Services.AddHostedService<TokenKeeperHostedService>();

if (config.RedisSettings.IsEnabled) builder.Services.AddHostedService<RedisIndexCreatorService>();

if (config.RedisSettings.IsEnabled) builder.Services.RegisterRedis(config);

builder.Services.RegisterRepositories();

builder.Services.AddMediatR(typeof(MediatrAssemblyContainer).Assembly);

builder.Services.AddHealthChecks();

builder.Services.RegisterAuthentication(config);

builder.Services.AddCors(o =>
    o.AddPolicy(Constants.EnableAllCorsName, b => b.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));

builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddEndpointsApiExplorer();

if (config.SwaggerSettings.IsEnabled) builder.Services.RegisterSwagger(config);

builder.Services.RegisterMiniProfiler();

builder.Services.AddSignalR();

var app = builder.Build();

// if (app.Environment.IsDevelopment())

if (config.SwaggerSettings.IsEnabled) app.UseSwagger();

if (config.SwaggerSettings.IsEnabled) app.UseSwaggerUI();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseCors(Constants.EnableAllCorsName);

app.UseMiddleware<LocalizationMiddleware>();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Frame-Options", "Deny");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    await next.Invoke();
});

if (config.SentrySettings.IsEnabled) app.UseSentryTracing();

app.UseStaticFiles();

app.UseAuthorization();

app.UseAuthentication();

app.UseMiniProfiler();

app.UseHealthChecks("/hc");

app.MapControllers();

app.MapHub<UserHub>("/userHub");

app.Run();