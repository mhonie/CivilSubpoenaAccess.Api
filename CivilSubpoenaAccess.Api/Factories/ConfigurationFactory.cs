using System.Diagnostics;
using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.Configuration;
using CivilSubpoenaAccess.Api.Environment;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Logging;
using CivilSubpoenaAccess.Api.Bootstrap;


#pragma warning disable CA1416

namespace CivilSubpoenaAccess.Api.Factories;


public interface IConfigurationFactory
{
    Result<IConfiguration, StartupError> Create();
}

public class ConfigurationFactory(StartupErrorTracker startupErrorTracker) 
    
    : EventViewerLogger, IConfigurationFactory
{
    
    private static void BuildingWebHostSettings()
    {
        LogEvent("Building web hosting settings...\n", EventLogEntryType.Information);
    }

    private void ErrorBuildingWeHostingSettings(StartupError startupError)
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

    public Result<IConfiguration, StartupError> Create()
    {
        BuildingWebHostSettings();

        try
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IEnvFile, EnvFile>();

            serviceCollection.AddSingleton<IEnvLoader, EnvLoader>();

            serviceCollection.AddSingleton<IEnvReader, EnvReader>();

            var configFileFactory = StartupServiceCallbacks.ConfigFile(startupErrorTracker);

            serviceCollection.AddSingleton(configFileFactory);

            serviceCollection.AddSingleton<IConfigurationBuilder, ConfigurationBuilder>();

            serviceCollection.AddSingleton<IConfigLoader, ConfigLoader>();

            var configurationFactory = StartupServiceCallbacks.Configuration(startupErrorTracker);

            serviceCollection.AddSingleton(configurationFactory);

            var serviceProvider = serviceCollection.BuildServiceProvider
            (
                new ServiceProviderOptions
                {
                    ValidateOnBuild = true,

                    ValidateScopes = true
                }
            );

            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            return Result.Success<IConfiguration, StartupError>(configuration);
        }
        catch (Exception startupException)
        {
            var startupError = startupErrorTracker.StartupError ?? new StartupError
            {
                InjectionError = new InjectionError
                {
                    Message = "Error building web hosting settings",

                    Exception = startupException
                }
            };

            ErrorBuildingWeHostingSettings(startupError);

            return startupError;
        }
    }
}