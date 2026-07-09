using CSharpFunctionalExtensions;
using DotNetEnv;
using System.Diagnostics;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Logging;

#pragma warning disable CA1416

namespace CivilSubpoenaAccess.Api.Environment;


/// <summary>
/// Loads the environment variables from the system and the .env file ONCE.
/// The .env variables override the variables set in the system environment.
/// </summary>
public interface IEnvLoader
{
    UnitResult<EnvFileError> Load();
}

/// <inheritdoc cref="IEnvLoader"/>
public class EnvLoader(IEnvFile envFile) : EventViewerLogger, IEnvLoader
{
    private bool isEnvLoaded;

    private readonly Result<string, EnvFileError> envFilePathResult = envFile.FilePath();

    // prevent loading twice when Load() called from multiple threads
    // not a problem for CivilSubpoenaAccess.Api, but is a problem for web APIs
    private readonly object loadLock = new();

    private static void LoadingEnvFile()
    {
        LogEvent("Loading .env file...\n", EventLogEntryType.Information);
    }

    private void ErrorLoadingEnvFile(EnvFileError envFileError)
    {
        if (envFileError.Exception is not null)

            LogEvent(envFileError.Exception.ToString(), EventLogEntryType.Error);

        string envFileErrorJson = ToJson(envFileError);

        var errorLoadingEnvFile = $"Error loading .env file\n\n{envFileErrorJson}\n";

        LogEvent(errorLoadingEnvFile, EventLogEntryType.Error);
    }

    public UnitResult<EnvFileError> Load()
    {
        try
        {
            LoadingEnvFile();

            if (isEnvLoaded)

                return UnitResult.Success<EnvFileError>();

            if (envFilePathResult.IsFailure)
            {
                var envFileError = envFilePathResult.Error;

                ErrorLoadingEnvFile(envFileError);

                return envFileError;
            }

            lock (loadLock)
            {
                if (isEnvLoaded)

                    return UnitResult.Success<EnvFileError>();

                string envFilePath = envFilePathResult.Value;
                
                if (!File.Exists(envFilePath))
                {
                    var missingEnvFileError = new EnvFileError
                    {
                        Message = "The .env file was not found",

                        FilePath = envFilePath
                    };

                    ErrorLoadingEnvFile(missingEnvFileError);

                    return missingEnvFileError;
                }

                Env.Load(envFilePath);

                isEnvLoaded = true;

                return UnitResult.Success<EnvFileError>();
            }
        }
        catch (Exception loadEnvFileException)
        {
            var loadEnvFileError = new EnvFileError
            {
                Message = "Error loading .env file",

                Exception = loadEnvFileException
            };

            ErrorLoadingEnvFile(loadEnvFileError);

            return loadEnvFileError;
        }
    }
}