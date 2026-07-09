using System.Diagnostics;
using CSharpFunctionalExtensions;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Logging;

#pragma warning disable CA1416

namespace CivilSubpoenaAccess.Api.Configuration;


/// <summary>
/// Loads the configuration from <c>appsettings.[Environment].json</c> files ONCE.
/// We use <c> Microsoft.Extensions.Configuration</c> to load the configuration.
/// </summary>
public interface IConfigLoader
{
    Result<IConfiguration, ConfigError> Load();
}

/// <inheritdoc cref="IConfigLoader"/>
public class ConfigLoader(IConfigFile configFile, IConfigurationBuilder configBuilder) 
    
    : EventViewerLogger, IConfigLoader
{
    private IConfiguration? configuration;

    private bool isLoaded;

    // prevent loading twice when Load() called from multiple threads
    // not a problem for CivilSubpoenaAccess.Api., but is a problem for web APIs
    private readonly object loadLock = new();

    private static void LoadingConfiguration()
    {
        LogEvent("Loading configuration...\n", EventLogEntryType.Information);
    }

    private void ErrorLoadingConfiguration(ConfigError configError)
    {
        if (configError.Exception is not null)

            LogEvent(configError.Exception.ToString(), EventLogEntryType.Error);

        string configErrorJson = ToJson(configError);

        var errorLoadingConfig = $"Error loading configuration\n\n{configErrorJson}\n";

        LogEvent(errorLoadingConfig, EventLogEntryType.Error);
    }

    public Result<IConfiguration, ConfigError> Load()
    {
        try
        {
            LoadingConfiguration();

            if (isLoaded)

                return Result.Success<IConfiguration, ConfigError>(configuration!);

            lock (loadLock)
            {
                if (isLoaded)

                    return Result.Success<IConfiguration, ConfigError>(configuration!);

                string configFileName = configFile.FileName;

                configuration = configBuilder

                    .SetBasePath(configFile.Directory)

                    .AddJsonFile(configFile.DefaultFileName, optional: false, reloadOnChange: false)

                    .AddJsonFile(configFileName, optional: false, reloadOnChange: false)

                    .Build();

                isLoaded = true;

                return Result.Success<IConfiguration, ConfigError>(configuration!);
            }
        }
        catch (Exception loadConfigException)
        {
            var loadConfigError = new ConfigError
            {
                Message = "Error loading config files",

                FileName = configFile.FileName,

                Exception = loadConfigException
            };

            ErrorLoadingConfiguration(loadConfigError);

            return loadConfigError;
        }
    }
}

