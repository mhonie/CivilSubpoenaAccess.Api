using CSharpFunctionalExtensions;
using System.Diagnostics;
using DotNetEnv;

using CivilSubpoenaAccess.Api.ErrorTypes;
using CivilSubpoenaAccess.Api.Logging;

#pragma warning disable CA1416

namespace CivilSubpoenaAccess.Api.Environment;


/// <summary>
/// Reads environment variables loaded by <see cref="EnvLoader"/>.
/// Uses a <c>Serilog</c> logger, if one is assigned at runtime.
/// Otherwise, logging output is written to Windows Event Viewer.
/// </summary>
public interface IEnvReader
{
    ILogger? Serilogger { set; }

    Result<string, EnvFileError> ReadValue(string variableName);
}

/// <inheritdoc cref="CivilSubpoenaAccess.Api.Environment.IEnvReader"/>
public class EnvReader : EventViewerLogger, IEnvReader
{
    public ILogger? Serilogger { get; set; }

    private void ReadingEnvironmentVariable(string variableName)
    {
        if (Serilogger is not null)

            Serilogger.Information("Reading the value of {VariableName}...\n", variableName);

        else
            
            LogEvent($"Reading the value of {variableName}...\n", EventLogEntryType.Information);
    }

    private void ErrorReadingEnvVariable(EnvFileError envFileError)
    {
        if(Serilogger is not null)
        {
            string envFileErrorJson = ToJson(envFileError);

            Serilogger.Error
            (
                envFileError.Exception,

                "Error getting environment variable\n\n{envFileError}\n",

                envFileErrorJson
            );
        }
        else
        {
            if (envFileError.Exception is not null)

                LogEvent(envFileError.Exception.ToString(), EventLogEntryType.Error);

            string envFileErrorJson = ToJson(envFileError);

            var errorGettingEnvVariable =

                $"Error getting environment variable\n\n{envFileErrorJson}\n";

            LogEvent(errorGettingEnvVariable, EventLogEntryType.Error);
        }
    }

    public Result<string, EnvFileError> ReadValue(string variableName)
    {
        try
        {
            ReadingEnvironmentVariable(variableName);

            string? variableValue =
                
                System.Environment.GetEnvironmentVariable(variableName);

            if (!string.IsNullOrEmpty(variableValue))

                return variableValue;

            variableValue = Env.GetString(variableName);

            if (!string.IsNullOrEmpty(variableValue))

                return variableValue;

            var envVariableError = new EnvFileError
            {
                Message = "Environment variable not set",

                VariableName = variableName
            };

            ErrorReadingEnvVariable(envVariableError);

            return envVariableError;
        }
        catch (Exception envVariableException)
        {
            var envVariableError = new EnvFileError
            {
                Message = "Error getting environment variable",

                VariableName = variableName,

                Exception = envVariableException
            };

            ErrorReadingEnvVariable(envVariableError);

            return envVariableError;
        }
    }
}