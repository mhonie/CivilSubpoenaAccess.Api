using System.Diagnostics;
using CSharpFunctionalExtensions;
using Destructurama;
using Serilog;
using Serilog.Settings.Configuration;

using CivilSubpoenaAccess.Api.ErrorTypes;

#pragma warning disable CA1416

namespace CivilSubpoenaAccess.Api.Logging;

/// <summary>
/// Creates a global logger ONCE. Logs are written according to the config in <c>appsettings.[Environment].json</c>.
/// </summary>
public interface ISerilogInstance
{
    Result<ILogger, LoggerError> Get();
}

public class SerilogInstance(IConfiguration configuration) : EventViewerLogger, ISerilogInstance
{
    private ILogger? serilogger;

    private readonly ConfigurationReaderOptions SerilogConfigOptions = new()
    {
        SectionName = nameof(Serilog)
    };

    // prevent loading twice when Create() called from multiple threads
    // not a problem for CivilSubpoenaAccess.Api., but is a problem for web APIs
    private readonly object factoryLock = new();

    private static void CreatingSerilogger()
    {
        const string creatingLogger = "Creating Serilog logger...\n";

        LogEvent(creatingLogger, EventLogEntryType.Information);
    }

    private void ErrorCreatingSerilogger(LoggerError loggerError)
    {
        if (loggerError.Exception is not null)

            LogEvent(loggerError.Exception.ToString(), EventLogEntryType.Error);

        string loggerErrorJson = ToJson(loggerError);

        var errorCreatingLogger = $"Error creating Serilog logger\n\n{loggerErrorJson}\n";

        LogEvent(errorCreatingLogger, EventLogEntryType.Error);
    }

    private Result<ILogger, LoggerError> Create()
    {
        try
        {
            CreatingSerilogger();

            ILogger newLogger = new LoggerConfiguration()

                .Destructure.UsingAttributes()

                .ReadFrom.Configuration(configuration, SerilogConfigOptions)

                .CreateLogger();

            return Result.Success<ILogger, LoggerError>(newLogger);
        }
        catch (Exception loggingException)
        {
            var loggerError = new LoggerError
            {
                Message = "Error creating base logger",

                Exception = loggingException
            };

            ErrorCreatingSerilogger(loggerError);

            return loggerError;
        }
    }

    public Result<ILogger, LoggerError> Get()
    {
        if (serilogger is not null)

            return Result.Success<ILogger, LoggerError>(serilogger);

        lock (factoryLock)
        {
            if (serilogger is not null)

                return Result.Success<ILogger, LoggerError>(serilogger);

            (bool _, bool seriloggerFailure, var newLogger, var seriloggerError)

                = Create();

            if (seriloggerFailure)

                return seriloggerError;

            serilogger = newLogger;

            return Result.Success<ILogger, LoggerError>(serilogger);
        }
    }
}