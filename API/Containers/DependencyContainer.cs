﻿using System.Text;
using System.Threading.RateLimiting;
using API.Hubs;
using BLL.Concrete;
using CORE.Abstract;
using CORE.Concrete;
using CORE.Config;
using CORE.Constants;
using CORE.ElasticSearch;
using CORE.Logging;
using DAL.Concrete;
using DAL.UnitOfWorks.Abstract;
using DAL.UnitOfWorks.Concrete;
using DTO.User;
using MediatR;
using MEDIATRS.MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Redis.OM;
using StackExchange.Profiling;
using StackExchange.Profiling.SqlFormatters;

namespace API.Containers;

public static class DependencyContainer
{
    public static void RegisterNLogger(this IServiceCollection services)
    {
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), Constants.NLogConfigPath));

        services.AddSingleton<ILoggerManager, LoggerManager>();
    }

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

    public static void RegisterRateLimit(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = 429; //default value is 503
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        QueueLimit = 2,
                        Window = TimeSpan.FromSeconds(10)
                    }));

            options.OnRejected = (_, _) => new ValueTask();
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(UserService).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(object)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scan => scan
            .FromAssemblies(typeof(UserRepository).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(object)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // util service is in the core assembly, therefore we need to register it separately
        services.AddScoped<IUtilService, UtilService>();
    }

    public static void RegisterSignalRHubs(this IServiceCollection services)
    {
        services.AddSingleton<UserHub>();
    }

    public static void RegisterAntiForgeryToken(this IServiceCollection services)
    {
        services.AddAntiforgery(options => { options.HeaderName = "X-XSRF-TOKEN"; });
    }

    public static void RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void RegisterOutputCache(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(builder => { builder.Expire(TimeSpan.FromMinutes(2)); });
        });
    }

    public static void RegisterRedis(this IServiceCollection services, ConfigSettings config)
    {
        services.AddSingleton(new RedisConnectionProvider(config.RedisSettings.Connection));
    }

    public static void RegisterHttpClients(this IServiceCollection services, ConfigSettings config)
    {
        services.AddHttpClient(config.FirstHttpClientSettings.Name, client =>
        {
            client.BaseAddress = new Uri(config.FirstHttpClientSettings.BaseUrl);
            client.Timeout = new TimeSpan(0, 0, config.FirstHttpClientSettings.TimeoutInSeconds);
            client.DefaultRequestHeaders.Clear();
            config.FirstHttpClientSettings.Headers.ForEach(h =>
                client.DefaultRequestHeaders.Add(h.Name, h.Value));
        });
    }

    public static void RegisterMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<MediatrAssemblyContainer>());

        services.Scan(scan =>
            scan.FromAssemblyOf<MediatrAssemblyContainer>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }

    public static void RegisterElasticSearch(this IServiceCollection services, ConfigSettings configs)
    {
        services.AddScoped<IElasticSearchService<UserToListDto>>(_ =>
            new ElasticSearchService<UserToListDto>(configs.ElasticSearchSettings.Connection,
                configs.ElasticSearchSettings.DefaultIndex));
    }

    public static void RegisterMiniProfiler(this IServiceCollection services)
    {
        services.AddMiniProfiler(options =>
        {
            // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

            // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
            options.RouteBasePath = "/profiler";

            options.ColorScheme = ColorScheme.Dark;

            // (Optional) Control storage
            // (default is 30 minutes in MemoryCacheStorage)
            // Note: MiniProfiler will not work if a SizeLimit is set on MemoryCache!
            //   See: https://github.com/MiniProfiler/dotnet/issues/501 for details
            //(options.Storage as MemoryCacheStorage)!.CacheDuration = TimeSpan.FromMinutes(60);
            options.SqlFormatter = new InlineFormatter();
        }).AddEntityFramework();
    }
}