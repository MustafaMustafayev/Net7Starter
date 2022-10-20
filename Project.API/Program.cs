using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.API.ActionFilters;
using Project.API.BackgroundServices;
using Project.API.DependencyContainers;
using Project.API.Hubs;
using Project.BLL.Mappers;
using Project.BLL.MediatR;
using Project.Core.Helper;
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

builder.WebHost.UseSentry();

builder.Services.AddAutoMapper(Automapper.GetAutoMapperProfilesFromAllAssemblies().ToArray());

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(config.ConnectionStrings.AppDb));

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

builder.Services.AddHostedService<TokenKeeperHostedService>();

builder.Services.RegisterRepositories();

// register unit of work after registering repositories
builder.Services.RegisterUnitOfWork();

builder.Services.AddMediatR(typeof(MediatrAssemblyContainer).Assembly);

builder.Services.AddHealthChecks();

builder.Services.RegisterAuthentication(config);

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b => b.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin()));

builder.Services.AddScoped<LogActionFilter>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.RegisterSwagger(config);

builder.Services.AddSignalR();

var app = builder.Build();

// if (app.Environment.IsDevelopment())

app.UseSwagger();
app.UseSwaggerUI();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseCors("CorsPolicy");

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

app.UseSentryTracing();

app.UseStaticFiles();

app.UseAuthorization();

app.UseAuthentication();

app.UseHealthChecks("/hc");

app.MapControllers();

app.MapHub<UserHub>("/userHub");

app.Run();