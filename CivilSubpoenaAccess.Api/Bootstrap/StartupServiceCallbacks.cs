using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.Configuration;
using CivilSubpoenaAccess.Api.Environment;
using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Logging;
using CivilSubpoenaAccess.Api.Factories;

namespace CivilSubpoenaAccess.Api.Bootstrap;


/// <summary>
/// Functions that tell <see cref="WebApplicationFactory"/> how to create
/// the objects that need to be created as soon as possible, i.e. the logger
/// </summary>
public interface IStartupServiceCallbacks
{
    Func<IServiceProvider, IConfigFile> ConfigFile
    (
        StartupErrorTracker startupErrorTracker
    );

    Func<IServiceProvider, IConfiguration> Configuration
    (
        StartupErrorTracker startupErrorTracker
    );

    Func<IServiceProvider, ILogger> Serilogger
    (
        StartupErrorTracker startupErrorTracker
    );
}

/// <inheritdoc cref="IStartupServiceCallbacks"/>
public class StartupServiceCallbacks : IStartupServiceCallbacks
{
    public required string EnvironmentNameKey { get; init; }

    public Func<IServiceProvider, IConfigFile> ConfigFile
    (
        StartupErrorTracker startupErrorTracker
    )
    {
        return serviceProvider =>
        {
            var envLoader = serviceProvider.GetRequiredService<IEnvLoader>();

            var loadResult = envLoader.Load();

            if (loadResult.IsFailure)
            {
                {
                    var initError = new InitializationError { EnvFileError = loadResult.Error };

                    var startupError = new StartupError { InitializationError = initError };

                    startupErrorTracker.StartupError = startupError;

                    throw new Exception(loadResult.Error.Message);
                }
            }

            var envReader = serviceProvider.GetRequiredService<IEnvReader>();

            (_, bool envNameFailure, string environmentName, var envNameError)

                = envReader.ReadValue(EnvironmentNameKey);

            if (!envNameFailure)
            {
                var configFile = new ConfigFile(environmentName);

                return configFile;
            }
            {
                var initError = new InitializationError { EnvFileError = envNameError };

                var startupError = new StartupError { InitializationError = initError };

                startupErrorTracker.StartupError = startupError;

                throw new Exception(envNameError.Message);
            }
        };
    }

    public Func<IServiceProvider, IConfiguration> Configuration
    (
        StartupErrorTracker startupErrorTracker
    )
    {
        return serviceProvider =>
        {
            var configLoader = serviceProvider.GetRequiredService<IConfigLoader>();

            (_, bool configurationFailure, var configuration, var configurationError) = 
                
                configLoader.Load();

            if (!configurationFailure) return configuration;

            var initError = new InitializationError { ConfigError = configurationError };

            var startupError = new StartupError { InitializationError = initError };

            startupErrorTracker.StartupError = startupError;

            throw new Exception(configurationError.Message);
        };
    }

    public Func<IServiceProvider, ILogger> Serilogger
    (
        StartupErrorTracker startupErrorTracker
    )
    {
        return serviceProvider =>
        {
            var serilogInstance = serviceProvider.GetRequiredService<ISerilogInstance>();

            (bool _, bool seriloggerFailure, ILogger serilogger, var seriloggerError)

                = serilogInstance.Get();

            if (!seriloggerFailure) return serilogger;

            var initError = new InitializationError { LoggerError = seriloggerError };

            var startupError = new StartupError { InitializationError = initError };

            startupErrorTracker.StartupError = startupError;

            throw new Exception(seriloggerError.Message);
        };
    }
}