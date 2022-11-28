using System.Text;
using BLL.Abstract;
using BLL.Concrete;
using CORE.Abstract;
using CORE.Concrete;
using CORE.Config;
using CORE.Constants;
using CORE.Logging;
using DAL.Abstract;
using DAL.Concrete;
using DAL.UnitOfWorks.Abstract;
using DAL.UnitOfWorks.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Redis.OM;
using StackExchange.Profiling;
using StackExchange.Profiling.SqlFormatters;

namespace API.Containers;

public static class StockDependencyContainer
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
                new OpenApiInfo
                    { Title = config.SwaggerSettings.Title, Version = config.SwaggerSettings.Version });

            c.AddSecurityDefinition(config.AuthSettings.TokenPrefix, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = config.AuthSettings.TokenPrefix,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
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
                }
            });
        });
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILoggingRepository, LoggingRepository>();
        services.AddScoped<ILoggingService, LoggingService>();
        services.AddScoped<IUtilService, UtilService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();

        // register unit of work after registering repositories
        services.RegisterUnitOfWork();
    }

    private static void RegisterUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
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
            config.FirstHttpClientSettings.Headers.ForEach(h => client.DefaultRequestHeaders.Add(h.Name, h.Value));
        });
    }

    public static void RegisterMiniProfiler(this IServiceCollection services)
    {
        services.AddMiniProfiler(options =>
        {
            // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

            // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
            options.RouteBasePath = "/profiler";

            options.ColorScheme = ColorScheme.Auto;

            // (Optional) Control storage
            // (default is 30 minutes in MemoryCacheStorage)
            // Note: MiniProfiler will not work if a SizeLimit is set on MemoryCache!
            //   See: https://github.com/MiniProfiler/dotnet/issues/501 for details
            //(options.Storage as MemoryCacheStorage)!.CacheDuration = TimeSpan.FromMinutes(60);

            // (Optional) Control which SQL formatter to use, InlineFormatter is the default
            options.SqlFormatter = new InlineFormatter();

            // (Optional) To control authorization, you can use the Func<HttpRequest, bool> options:
            // (default is everyone can access profilers)
            //options.ResultsAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;
            //options.ResultsListAuthorize = request => MyGetUserFunction(request).CanSeeMiniProfiler;
            // Or, there are async versions available:
            //options.ResultsAuthorizeAsync = async request => (await MyGetUserFunctionAsync(request)).CanSeeMiniProfiler;
            //options.ResultsAuthorizeListAsync = async request => (await MyGetUserFunctionAsync(request)).CanSeeMiniProfilerLists;

            // (Optional)  To control which requests are profiled, use the Func<HttpRequest, bool> option:
            // (default is everything should be profiled)
            //options.ShouldProfile = request => MyShouldThisBeProfiledFunction(request);

            // (Optional) Profiles are stored under a user ID, function to get it:
            // (default is null, since above methods don't use it by default)
            //options.UserIdProvider =  request => MyGetUserIdFunction(request);

            // (Optional) Swap out the entire profiler provider, if you want
            // (default handles async and works fine for almost all applications)
            //options.ProfilerProvider = new MyProfilerProvider();

            // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
            // (defaults to true, and connection opening/closing is tracked)
            //options.TrackConnectionOpenClose = true;

            // Optionally change the number of decimal places shown for millisecond timings.
            // (defaults to 2)
            //options.PopupDecimalPlaces = 1;

            // The below are newer options, available in .NET Core 3.0 and above:

            // (Optional) You can disable MVC filter profiling
            // (defaults to true, and filters are profiled)
            //options.EnableMvcFilterProfiling = true;
            // ...or only save filters that take over a certain millisecond duration (including their children)
            // (defaults to null, and all filters are profiled)
            // options.MvcFilterMinimumSaveMs = 1.0m;

            // (Optional) You can disable MVC view profiling
            // (defaults to true, and views are profiled)
            //options.EnableMvcViewProfiling = true;
            // ...or only save views that take over a certain millisecond duration (including their children)
            // (defaults to null, and all views are profiled)
            // options.MvcViewMinimumSaveMs = 1.0m;

            // (Optional) listen to any errors that occur within MiniProfiler itself
            // options.OnInternalError = e => MyExceptionLogger(e);

            // (Optional - not recommended) You can enable a heavy debug mode with stacks and tooltips when using memory storage
            // It has a lot of overhead vs. normal profiling and should only be used with that in mind
            // (defaults to false, debug/heavy mode is off)
            //options.EnableDebugMode = true;
        }).AddEntityFramework();
    }
}