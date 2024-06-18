using BLL;
using CORE.Abstract;
using CORE.Concrete;
using CORE.Config;
using DAL.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Refit;
using REFITS.Clients;
using StackExchange.Profiling;
using StackExchange.Profiling.SqlFormatters;
using System.Text;

namespace API.Containers;

public static class DependencyContainer
{
    public static void RegisterAuthentication(this IServiceCollection services, ConfigSettings config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.AuthSettings.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            options.Lockout.AllowedForNewUsers = true;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
            options.SlidingExpiration = true;

            options.Cookie = new CookieBuilder
            {
                HttpOnly = true,
                Name = ".AspNetCore.Security.Cookie",
                SameSite = SameSiteMode.Lax,
                SecurePolicy = CookieSecurePolicy.SameAsRequest
            };
        });
    }

    public static void RegisterSwagger(this IServiceCollection services, ConfigSettings config)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();

            c.SwaggerDoc(config.SwaggerSettings.Version,
                new OpenApiInfo { Title = config.SwaggerSettings.Title, Version = config.SwaggerSettings.Version });

            c.AddSecurityDefinition(config.AuthSettings.TokenPrefix, new OpenApiSecurityScheme
            {
                Name = config.AuthSettings.HeaderName,
                Type = SecuritySchemeType.ApiKey,
                Scheme = config.AuthSettings.TokenPrefix,
                BearerFormat = config.AuthSettings.Type,
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityDefinition(config.AuthSettings.RefreshTokenHeaderName, new OpenApiSecurityScheme
            {
                Name = config.AuthSettings.RefreshTokenHeaderName,
                In = ParameterLocation.Header,
                Description = "Refresh token header."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = config.AuthSettings.TokenPrefix
                        }
                    },
                    Array.Empty<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = config.AuthSettings.RefreshTokenHeaderName
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void RegisterApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ReportApiVersions = true;
            opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version"));
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.TryAddScoped<IUtilService, UtilService>();

        services.Scan(scan => scan
            .FromAssemblies(typeof(Service).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(object)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(typeof(Repository).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(object)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    public static void RegisterMiniProfiler(this IServiceCollection services)
    {
        services.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
            options.ColorScheme = ColorScheme.Dark;
            options.SqlFormatter = new InlineFormatter();
        }).AddEntityFramework();
    }

    public static void RegisterRefitClients(this IServiceCollection services, ConfigSettings config)
    {
        services
            .AddRefitClient<IToDoClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(config.ToDoClientSettings.BaseUrl));
    }
}