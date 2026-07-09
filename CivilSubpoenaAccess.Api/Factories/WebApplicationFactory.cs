using System.Diagnostics;
using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.Bootstrap;
using CivilSubpoenaAccess.Api.Database;
using CivilSubpoenaAccess.Api.Database.Repositories;
using CivilSubpoenaAccess.Api.Database.Schema.Tables;
using CivilSubpoenaAccess.Api.Environment;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Logging;
using CivilSubpoenaAccess.Api.Reports;
using CivilSubpoenaAccess.Api.Reports.Pagination;
using CivilSubpoenaAccess.Api.Reports.UserTypes;
using CivilSubpoenaAccess.Api.WebHosting;
using CivilSubpoenaAccess.Api.Reports.Queries.LineItems;
using CivilSubpoenaAccess.Api.Reports.Queries.Aggregates;

#pragma warning disable CA1416

namespace CivilSubpoenaAccess.Api.Factories;


/// <summary>
/// Create every object all at once and inject them into the <see cref="WebApplication"/>.
/// Write logging output to Windows Event Viewer since <c>Serilog</c> is not available yet.
/// </summary>
public interface IWebApplicationFactory
{
    Result<WebApplication, StartupError> Create(string[] commandLineArgs);
}

/// <inheritdoc cref="IWebApplicationFactory"/>
public class WebApplicationFactory(IConfiguration configuration, StartupErrorTracker startupErrorTracker) 
    
    : EventViewerLogger, IWebApplicationFactory
{
    /// <summary>
    /// Gets objects from the config based on the name of the type
    /// </summary>
    /// <typeparam name="T"><c>T</c> should be a POCO</typeparam>
    /// <returns>A config object of type <c>T</c></returns>
    private static Action<T, IConfiguration> GetSettings<T>() where T : class
    {
        return (settings, configuration) =>
        {
            string sectionName = typeof(T).Name;

            configuration.GetSection(sectionName).Bind(settings);
        };
    }

    private WebHostSettings WebHostSettings()
    {
        var webHostSettings = configuration

            .GetSection(nameof(WebHostSettings))

            .Get<WebHostSettings>();

        if (webHostSettings is null)

            throw new Exception("Missing web host settings");

        return webHostSettings;
    }

    private static void BuildingWebApplication()
    {
        LogEvent("Building web application...\n", EventLogEntryType.Information);
    }

    private void ErrorBuildingWebApplication(StartupError startupError)
    {
        var startupException = new Exception();

        var initError = startupError.InitializationError;

        var injectionError = startupError.InjectionError;

        if (initError is not null)
        {
            startupException = initError.EnvFileError?.Exception

                               ?? initError.ConfigError?.Exception

                               ?? initError.LoggerError?.Exception;
        }

        if (injectionError is not null)
        
            startupException = injectionError.Exception;

        if (startupException is not null)

            LogEvent(startupException.ToString(), EventLogEntryType.Error);

        string startupErrorJson = ToJson(startupError);

        var errorBuildingPipeline = $"Error building web application\n\n{startupErrorJson}\n";

        LogEvent(errorBuildingPipeline, EventLogEntryType.Error);
    }

    public required IStartupServiceCallbacks StartupServiceCallbacks { get; init; }

    public required IEntityFrameworkCallbacks EntityFrameworkCallbacks { get; init; }

    private const string HealthCheckRoute = "/health";

    private const string ReactCorsPolicy = "React";

    public Result<WebApplication, StartupError> Create(string[] commandLineArgs)
    {
        BuildingWebApplication();

        try
        {
            var webAppBuilder = WebApplication.CreateBuilder(commandLineArgs);

            webAppBuilder.Services.AddSingleton<IEnvReader, EnvReader>();

            webAppBuilder.Services.AddSingleton(configuration);

            webAppBuilder.Services.AddSingleton<ISerilogInstance, SerilogInstance>();

            var seriloggerInstance = StartupServiceCallbacks.Serilogger(startupErrorTracker);

            webAppBuilder.Services.AddSingleton(seriloggerInstance);

            webAppBuilder.Services.AddOptions<EfilingConnectionString>()

                .Configure(GetSettings<EfilingConnectionString>())

                .ValidateDataAnnotations()

                .ValidateOnStart();

            webAppBuilder.Services.AddSingleton<IConnectionStringBuilder, ConnectionStringBuilder>();

            webAppBuilder.Services.AddSingleton<ICaseTable, CaseTable>();

            webAppBuilder.Services.AddSingleton<ICartTable, CartTable>();

            webAppBuilder.Services.AddSingleton<IUserTable, UserTable>();

            webAppBuilder.Services.AddDbContextFactory<EFilingDatabaseContext>
            (
                EntityFrameworkCallbacks.DbContextOptions(startupErrorTracker)
            );

            webAppBuilder.Services.AddScoped<ICartRepository, CartRepository>();

            webAppBuilder.Services.AddScoped<ICaseRepository, CaseRepository>();

            webAppBuilder.Services.AddScoped<IUserRepository, UserRepository>();

            webAppBuilder.Services.AddScoped<ISubpoenaInfoQueries, SubpoenaInfoQueries>();

            webAppBuilder.Services.AddScoped<ITransactionsBySubpoenaInfo, TransactionsBySubpoenaInfo>();

            webAppBuilder.Services.AddScoped<IPaymentInfoQueries, PaymentInfoQueries>();

            webAppBuilder.Services.AddScoped<ITransactionsByPaymentInfo, TransactionsByPaymentInfo>();

            webAppBuilder.Services.AddScoped<IUserTypes, UserTypes>();

            webAppBuilder.Services.AddScoped<IUserInfoQueries, UserInfoQueries>();

            webAppBuilder.Services.AddScoped<ITransactionsByUserInfo, TransactionsByUserInfo>();

            webAppBuilder.Services.AddHttpContextAccessor();

            webAppBuilder.Services.AddGraphQLServer()
                
                .AddQueryType<SubpoenaReports>()

                .AddType<PaymentTypePagination>()

                .AddType<SubpoenaTypePagination>()

                .AddType<SubpoenaStatusPagination>()

                .AddType<UserTypePagination>()

                .AddFiltering()
                
                .AddSorting()
                
                .AddProjections();

            var webHostSettings = WebHostSettings();

            string[] AllowedOrigins = webHostSettings.CorsSettings.AllowedOrigins;

            webAppBuilder.Services.AddHealthChecks();

            webAppBuilder.Services.AddCors
            (
                corsOptions => {

                    corsOptions.AddPolicy
                    (
                        ReactCorsPolicy,

                        corsPolicy => {

                            corsPolicy.AllowAnyHeader()

                                .AllowAnyMethod()

                                .WithOrigins(AllowedOrigins);
                        }
                    );
                }
            );

            webAppBuilder.Host.UseDefaultServiceProvider(options =>
            {
                options.ValidateOnBuild = true;

                options.ValidateScopes = true;
            });

            var webApplication = webAppBuilder.Build();

            webApplication.MapHealthChecks(HealthCheckRoute);

            webApplication.MapGraphQL();

            webApplication.UseCors(ReactCorsPolicy);

            return Result.Success<WebApplication, StartupError>(webApplication);
        }
        catch (Exception startupException)
        {
            var startupError = startupErrorTracker.StartupError ?? new StartupError
            {
                InjectionError = new InjectionError
                {
                    Message = "Error building web application",

                    Exception = startupException
                }
            };

            ErrorBuildingWebApplication(startupError);

            return startupError;
        }
    }
}